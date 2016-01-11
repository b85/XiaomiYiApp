using Newtonsoft.Json;
using PrismClone.StoreApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PrismClone.StoreApp
{
    public class FrameNavigationService : INavigationService
    {
        /// <summary> 
        /// The view model routing. 
        /// </summary> 
        //private static readonly Dictionary<System.Type, string> ViewModelRouting = new Dictionary<System.Type, string> 
        //                                                            { 
        //                                                                  //{ 
        //                                                                  //    typeof(TitlesViewModel), "View/TitlesPage.xaml" 
        //                                                                  //}, 
        //                                                                 // { 
        //                                                                 //     typeof(TitleDetailsViewModel), "View/TitleDetailsPage.xaml" 
        //                                                                 //} 
        //                                                             };


        /*
        private static readonly Dictionary<PagesID, string> ViewModelRouting = new Dictionary<PagesID, string>
                                                                    {
                                                                        { 
                                                                            PagesID.LoginPage, "LoginPage.xaml" 
                                                                        }, 
                                                                         { 
                                                                            PagesID.PageCategorie, "MainPage.xaml" 
                                                                        } ,
                                                                        {
                                                                            PagesID.SearchPage, "SearchPage.xaml"
                                                                        },
                                                                        {
                                                                            PagesID.PageArticoli, "ArticoliPage.xaml"
                                                                        },
                                                                        {
                                                                            PagesID.PageDettaglio, "DettaglioPage.xaml"
                                                                        }

                                                                    };
         */
        private const String URI_PARAMITER_IDENTIFIER = "?param=";

        private Frame _rootFrame;
        private readonly Func<Type, String> _navigationResolver;

        public FrameNavigationService(Frame rootFrame,  Func<Type, String> navigationResolver)
        {
            _navigationResolver = navigationResolver;
            _rootFrame = rootFrame;
            _rootFrame.Navigated += _rootFrame_Navigated;
            _rootFrame.Navigating += _rootFrame_Navigating;
        }

        /// <summary> 
        /// Gets a value indicating whether can go back. 
        /// </summary> 
        public bool CanGoBack
        {
            get
            {
                return _rootFrame.CanGoBack;
            }
        }

        /// <summary> 
        /// Gets the root frame. 
        /// </summary> 
        //private Frame _rootFrame
        //{
        //    get { return Application.Current.RootVisual as Frame; }
        //}

        /// <summary> 
        /// Decodes the navigation parameter. 
        /// </summary> 
        /// <typeparam name="TJson">The type of the json.</typeparam> 
        /// <param name="context">The context.</param> 
        /// <returns>The json result.</returns> 
        public  TJson DecodeNavigationParameter<TJson>(NavigationContext context)
        {
            if (context.QueryString.ContainsKey("param"))
            {
                var param = context.QueryString["param"];
                return  string.IsNullOrWhiteSpace(param) ? default(TJson) : JsonConvert.DeserializeObject<TJson>(param);
            }

            throw new KeyNotFoundException();
        }

        private  String GetPageUrl(Type viewModelType)
        {
            /*
            if (!viewModelType.Name.EndsWith("model", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("ViewModel type name must end with Model");
            }
            return String.Format(@"/Views/{0}.xaml", viewModelType.Name.Remove(viewModelType.Name.Length - 5));
            */

            return _navigationResolver(viewModelType);
        }

        /// <summary> 
        /// The go back. 
        /// </summary> 
        public void GoBack()
        {
            _rootFrame.GoBack();
        }

        /// <summary> 
        /// Navigates the specified parameter. 
        /// </summary> 
        /// <typeparam name="TDestinationViewModel">The type of the destination view model.</typeparam> 
        /// <param name="parameter">The parameter.</param> 
        //public void Navigate<TDestinationViewModel>(object parameter)
        //{

        //    var navParameter = string.Empty;
        //    if (parameter != null)
        //    {
        //        navParameter = "?param=" + JsonConvert.SerializeObject(parameter);
        //    }

        //    if (ViewModelRouting.ContainsKey(typeof(TDestinationViewModel)))
        //    {
        //        var page = ViewModelRouting[typeof(TDestinationViewModel)];

        //        this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
        //    }
        //} 


        public void Navigate(Type viewModelType, object parameter)
        {

            String navParameter = string.Empty;
            String pageUrl = GetPageUrl(viewModelType);

            if (parameter != null)
            {
                Type typeName = parameter.GetType();
                navParameter = URI_PARAMITER_IDENTIFIER + JsonConvert.SerializeObject(parameter) + "&type=" + typeName.FullName;
            }

            this._rootFrame.Navigate(new Uri(String.Concat(pageUrl ,navParameter), UriKind.Relative));
            //if (ViewModelRouting.ContainsKey(pageID))
            //{
            //    var page = ViewModelRouting[pageID];

            //    this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            //}
        }

        //private String GetParameter(Uri  uri)
        //{
        //    if (context.QueryString.ContainsKey("param"))
        //}

        private void NavigateFromCurrentViewModel()
        {
            FrameworkElement departingView = _rootFrame.Content as FrameworkElement;
            if (departingView == null) return;
            INavigationAware departingViewModel = departingView.DataContext as INavigationAware;
            if (departingViewModel != null)
            {
                departingViewModel.OnNavigatedFrom(null, false);
            }
        }

        private void NavigateToCurrentViewModel(NavigationMode navigationMode, object parameter)
        { 
             var newView = _rootFrame.Content as FrameworkElement;
            if (newView == null) return;
            var newViewModel = newView.DataContext as INavigationAware;
            if (newViewModel != null)
            {
                newViewModel.OnNavigatedTo(parameter, navigationMode, null);
            }
        }


        void _rootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            NavigateFromCurrentViewModel(); 
        }

        void _rootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //TODO parameter
            var u = e.Uri;
            Object param = null;
            var d = ParseQueryString(e.Uri.ToString());
            if (d.Count == 2)
            {
                Type paramType = Type.GetType(d["type"]);
                param = JsonConvert.DeserializeObject(d["param"], paramType);
            }
            NavigateToCurrentViewModel(e.NavigationMode, param);
        }


        public  Dictionary<string, string> ParseQueryString(string uri)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            int queryStringStartIndex = uri.IndexOf('?');
            if (queryStringStartIndex != -1)
            {
               // string substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));

               // string[] pairs = substring.Split('&');

                string[] pairs = uri.Substring(queryStringStartIndex + 1).Split('&');
                foreach (string piece in pairs)
                {
                    string[] pair = piece.Split('=');
                    output.Add(pair[0], pair[1]);
                }
            }

            return output;

        }

    }
}
