using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Servicies.Interfaces
{
    public interface INavigationService
    {
        /// <summary> 
        /// Gets a value indicating whether can go back. 
        /// </summary> 
        bool CanGoBack { get; }

        /// <summary> 
        /// The go back. 
        /// </summary> 
        void GoBack();

        /// <summary> 
        /// The navigate. Non posso usarlo perchè viewmodel e view nonsono nello stesso assembly. Dovrei mettere una dipendenza incrociata. 
        /// </summary> 
        /// <param name="parameter"> 
        /// The parameter. 
        /// </param> 
        /// <typeparam name="TDestinationPage"> 
        /// The destination view model. 
        /// </typeparam> 
        //void Navigate<TDestinationPage>(object parameter = null);

        void Navigate(Type pageType, object parameter = null);
    }
}
