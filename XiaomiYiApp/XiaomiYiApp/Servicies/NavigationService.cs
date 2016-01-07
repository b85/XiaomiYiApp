using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class NavigationService : INavigationService
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

        /// <summary> 
        /// Gets a value indicating whether can go back. 
        /// </summary> 
        public bool CanGoBack
        {
            get
            {
                return RootFrame.CanGoBack;
            }
        }

        /// <summary> 
        /// Gets the root frame. 
        /// </summary> 
        private Frame RootFrame
        {
            get { return Application.Current.RootVisual as Frame; }
        }

        /// <summary> 
        /// Decodes the navigation parameter. 
        /// </summary> 
        /// <typeparam name="TJson">The type of the json.</typeparam> 
        /// <param name="context">The context.</param> 
        /// <returns>The json result.</returns> 
        public static TJson DecodeNavigationParameter<TJson>(NavigationContext context)
        {
            if (context.QueryString.ContainsKey("param"))
            {
                var param = context.QueryString["param"];
                return  string.IsNullOrWhiteSpace(param) ? default(TJson) : JsonConvert.DeserializeObject<TJson>(param);
            }

            throw new KeyNotFoundException();
        }

        private static String GetPageUrl(Type viewModelType)
        {
            if (!viewModelType.Name.EndsWith("model", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("ViewModel type name must end with Model");
            }
            return String.Format(@"/Views/{0}.xaml", viewModelType.Name.Remove(viewModelType.Name.Length - 5));
        }

        /// <summary> 
        /// The go back. 
        /// </summary> 
        public void GoBack()
        {
            RootFrame.GoBack();
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
                navParameter = "?param=" + JsonConvert.SerializeObject(parameter);
            }

            this.RootFrame.Navigate(new Uri(String.Concat(pageUrl ,navParameter), UriKind.Relative));
            //if (ViewModelRouting.ContainsKey(pageID))
            //{
            //    var page = ViewModelRouting[pageID];

            //    this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            //}
        } 

    }
}
