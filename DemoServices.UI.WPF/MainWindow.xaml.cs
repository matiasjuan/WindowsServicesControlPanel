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

            CheckStatus();
        }

        /*
        private Cursor defaultCursor;

        public void si_DoStopService(object sender, EventArgs e)
        {
            UIServiceItem item = (UIServiceItem)sender;
            ServiceMonitorHandler smh = new ServiceMonitorHandler(item.ServiceItem, this);
            item.ShowAsDisabled();
            defaultCursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            smh.Stop();
        }

        public void si_DoRunService(object sender, EventArgs e)
        {
            UIServiceItem item = (UIServiceItem)sender;
            ServiceMonitorHandler smh = new ServiceMonitorHandler(item.ServiceItem, this);
            item.ShowAsDisabled();
            defaultCursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            smh.Start();
        }

        public void si_DoRestartService(object sender, EventArgs e)
        {
            UIServiceItem item = (UIServiceItem)sender;
            ServiceMonitorHandler smh = new ServiceMonitorHandler(item.ServiceItem, this);
            item.ShowAsDisabled();
            defaultCursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            smh.Restart();
        }

        public void ServiceStatusChanged(ServiceController service)
        {
            this.Cursor = defaultCursor;
            CheckStatus();
        }

        public void ServiceStatusError(ServiceMonitor serviceMonitor, String message)
        {
            this.Cursor = defaultCursor;
            MessageBox.Show(String.Format("Servicio {0}, error {1}", serviceMonitor.Service.Controller.ServiceName, message));
            CheckStatus();
        }

        public object PerformInvoke(Delegate method, params object[] args)
        {
            return this.Dispatcher.Invoke(method, args);
        }
        */
        
        private void CheckStatus()
        {
            foreach (var serviceItem in groups.ServiceItems)
            {
                //serviceItem.Value.CheckStatus();
                
            }
            /*
            foreach (var item in groups.ServiceItems)
            {
                if ( item.Value.Exists )
                {
                    foreach (var control in item.Value.Controls)
                    {

                        ServiceController service = new ServiceController(item.Key);
                        UIServiceItem si = (UIServiceItem)control;
                        UpdateUIStatus(si, service);
                    }
                }
            }
             * */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckStatus();
        }

        private void btnStopAll_Click(object sender, RoutedEventArgs e)
        {
            /*
            defaultCursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;
            btnStopAll.IsEnabled = false;
            try
            {
                foreach (var item in groups.ServiceItems)
                {
                    if (item.Value.Exists)
                    {
                        ServiceController service = new ServiceController(item.Key);
                        if (service.CanStop)
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = defaultCursor;
                btnStopAll.IsEnabled = true;
            }
            CheckStatus();
             */
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
