using DemoServices.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DemoServices.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceItemListener
    {
        public MainWindow()
        {
            InitializeComponent();

            ProgressBarHide();

            if (groups == null)
                LoadAll();
        }

        private ServiceGroups groups;

        private void LoadAll()
        {
            stack.Children.Clear();
            
            groups = new ServiceGroups();

            //Load xml
            groups.LoadFromFile("config.xml");

            foreach (var g in groups.Groups)
            {
                UIExpander expander = new UIExpander();
                stack.Children.Add(expander);
                expander.LoadGroup(g.Value);
            }
            
            foreach(var serviceItem in groups.ServiceItems)
            {
                serviceItem.Value.AddListener(this);
            }

        }

        public void RefreshStatus(ServiceItem serviceItem)
        {
            //throw new NotImplementedException();
        }

        public void ServiceStatusChanging(ServiceItem serviceItem)
        {
            //this.ShowAsDisabled();
        }

        public void ServiceStarted(ServiceItem serviceItem)
        {
            ProgressBarHide();
        }

        public void ServiceStopped(ServiceItem serviceItem)
        {
            ProgressBarHide();
        }

        public void ServiceRestarted(ServiceItem serviceItem)
        {
            ProgressBarHide();
        }

        public void ServiceError(ServiceItem serviceItem, string error)
        {
            MessageBox.Show(error);
            ProgressBarHide();
        }

        public void ServiceStatusChangeProgress(ServiceItem serviceItem, long ticks)
        {
            ProgressBarProgress(TimeSpan.FromTicks(ticks).TotalMilliseconds);
        }

        public object PerformInvoke(Delegate method, params object[] args)
        {
            return this.Dispatcher.Invoke(method, args);
        }

        #region ProgressBar
        private void ProgressBarHide()
        {
            //this.progressBar.Visibility = System.Windows.Visibility.Hidden;
            panelProgress.Visibility = System.Windows.Visibility.Hidden;
        }
        private void ProgressBarProgress(double milliseconds)
        {
            progressBar.Maximum = 60 * 1000;
            progressBar.Minimum = 0;
            panelProgress.Visibility = System.Windows.Visibility.Visible;
            progressBar.Value = milliseconds;
        }
        #endregion

    }
}
