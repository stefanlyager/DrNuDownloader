using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace DrNuDownloader.Wpf
{
    public class AutoWireUpViewModelBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            //var view = (FrameworkElement)this.AssociatedObject;
            //var viewModelName = string.Format("{0}Model", view.GetType().FullName);
            //var viewModel = ServiceLocator.IoC.Resolve<object>(viewModelName);
            //view.DataContext = viewModel;
        }
    }
}