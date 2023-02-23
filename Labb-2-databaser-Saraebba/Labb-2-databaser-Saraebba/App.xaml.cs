using Labb_2_databaser_Saraebba.Managers;
using Labb_2_databaser_Saraebba.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Labb_2_databaser_Saraebba
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly BokhandelManager _bokhandelManager;

        public App()
        {
            _navigationManager = new NavigationManager();
            _bokhandelManager = new BokhandelManager();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new MenyViewModel(_navigationManager, _bokhandelManager);
            MainWindow = new MainWindow
            {
                DataContext = new NavigationViewModel(_navigationManager)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
