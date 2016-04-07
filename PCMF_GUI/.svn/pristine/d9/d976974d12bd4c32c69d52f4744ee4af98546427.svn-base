using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using IPS.Views;


namespace IPS
{
    //The Bootstrapper is responsible for application initialization, registering of the main view and registering of the modules. 
    public class Bootstrapper : UnityBootstrapper
    {

        protected override DependencyObject CreateShell()
        {
            Shell shell = this.Container.Resolve<Shell>();
            Application.Current.RootVisual = shell;
            return (shell);
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            Uri uri = new Uri("catalog.xaml", UriKind.Relative);
            return ModuleCatalog.CreateFromXaml(uri);
        }
    }
}
