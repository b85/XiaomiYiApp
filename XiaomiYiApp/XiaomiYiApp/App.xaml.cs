using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XiaomiYiApp.Resources;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using XiaomiYiApp.ViewModels;
using XiaomiYiApp.Servicies.Interfaces;
using XiaomiYiApp.Servicies;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Repositories;
using Microsoft.Practices.Prism.PubSubEvents;
using PrismClone.StoreApp.Interfaces;
using PrismClone.StoreApp;

namespace XiaomiYiApp
{
    public partial class App : Application
    {
        private readonly IUnityContainer _container = new UnityContainer();

        private EventAggregator _eventAggregator;

        private INavigationService _navigationService;

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            _eventAggregator = new EventAggregator();
            _navigationService = new FrameNavigationService(RootFrame, (viewModelType) =>
            {
                if (!viewModelType.Name.EndsWith("model", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("ViewModel type name must end with Model");
                }
                return String.Format(@"/Views/{0}.xaml", viewModelType.Name.Remove(viewModelType.Name.Length - 5));
            });

            //unity
            _container.RegisterInstance<IEventAggregator>(_eventAggregator, new ContainerControlledLifetimeManager());
            _container.RegisterInstance<INavigationService>(_navigationService, new ContainerControlledLifetimeManager());
            //_container.RegisterType<INavigationService, XiaomiYiApp.Servicies.NavigationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMsgBoxService, MsgBoxService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICameraConnectionService, CameraConnectionService>(new ContainerControlledLifetimeManager());
            // _container.RegisterType<ICameraNotificationService, CameraNotificationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICameraConfigurationService, CameraConfigurationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICameraConfigurationRepository, CameraConfigurationRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICaneraStateRepository, CaneraStateRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ConnectViewModel>();
            _container.RegisterType<MainViewModel>();
            _container.RegisterType<ConfigurationViewModel>();

            //registro istanza perchè per ora non è richiamato da nessuna entità
            _container.RegisterInstance<ICameraNotificationService>(new CameraNotificationService(_container.Resolve<ICameraConnectionService>(), _eventAggregator));

            //_container.RegisterInstance<ISessionStateService>(SessionStateService);

            // Set a factory for the ViewModelLocator to use the container to construct view models so their 
            // dependencies get injected by the container
            //  ViewModelLocator.SetDefaultViewModelFactory((viewModelType) => _container.Resolve(viewModelType));

            // prism
            ViewModelLocationProvider.SetDefaultViewModelFactory((viewModelType) =>
                _container.Resolve(viewModelType));

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = String.Format("XiaomiYiApp.ViewModels.{0}Model", viewType.Name);
                //string.Format(CultureInfo.InvariantCulture, "AdventureWorks.UILogic.ViewModels.{0}ViewModel, AdventureWorks.UILogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=634ac3171ee5190a", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}