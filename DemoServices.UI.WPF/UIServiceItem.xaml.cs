using DemoServices.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoServices.UI.WPF
{
    /// <summary>
    /// Interaction logic for UIServiceItem.xaml
    /// </summary>
    public partial class UIServiceItem : UserControl, ServiceItemListener
    {
        public UIServiceItem()
        {
            InitializeComponent();
        }

        private ServiceItem serviceItem;

        public ServiceItem ServiceItem
        {
            get { 
                return serviceItem; 
            }
            set
            {
                this.serviceItem = value;
                this.lbl.Content = value.ServiceName;
                serviceItem.AddListener(this);
                this.DetermeHowToShow();
            }
        }

        public String ServiceName { 
            get {
                return serviceItem.ServiceName;
            } 
        }

        #region Change buttons style
        private void HideControl(Control control)
        {
            Storyboard storyBoard = (Storyboard)FindResource("animateHide");
            storyBoard.Begin(control);
        }
        private void ShowControl(Control control)
        {
            Storyboard storyBoard = (Storyboard)FindResource("animateShow");
            storyBoard.Begin(control);
        }

        internal void ShowAsDisabled()
        {
            HideControl(btnStart);
            HideControl(btnStop);
            //HideControl(btnRestart);
        }

        internal void ShowAsReadyToStart()
        {
            ShowControl(btnStart);
            HideControl(btnStop);
            //HideControl(btnRestart);
        }

        internal void ShowAsReadyToStop()
        {
            HideControl(btnStart);
            ShowControl(btnStop);
            //ShowControl(btnRestart);
        }

        private void DetermeHowToShow()
        {
            if (this.serviceItem.Controller.Status.Equals(System.ServiceProcess.ServiceControllerStatus.Running))
                this.ShowAsReadyToStop();
            if (this.serviceItem.Controller.Status.Equals(System.ServiceProcess.ServiceControllerStatus.Stopped))
                this.ShowAsReadyToStart();
        }
        #endregion

        //public event EventHandler DoRunService;
        //public event EventHandler DoStopService;
        //public event EventHandler DoRestartService;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.serviceItem.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.serviceItem.Stop();
        }


        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            //if (DoRestartService != null)
            //    DoRestartService(this, new EventArgs());
        }

        public void ServiceStatusChanging(ServiceItem serviceItem)
        {
            this.ShowAsDisabled();
        }

        public void ServiceStarted(ServiceItem serviceItem)
        {
            this.ShowAsReadyToStop();
        }

        public void ServiceStopped(ServiceItem serviceItem)
        {
            this.ShowAsReadyToStart();
        }

        public void ServiceRestarted(ServiceItem serviceItem)
        {
            this.ShowAsReadyToStop();
        }

        public void ServiceError(ServiceItem serviceItem, string error)
        {
            DetermeHowToShow();
        }

        public void ServiceStatusChangeProgress(ServiceItem serviceItem, long ticks)
        {
            //
        }

        public object PerformInvoke(Delegate method, params object[] args)
        {
            return this.Dispatcher.Invoke(method, args);
        }

        public void RefreshStatus(ServiceItem serviceItem)
        {
            DetermeHowToShow();
        }
    }
}
