using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoServices.UI.Common
{
    public interface ServiceItemListener
    {
        void ServiceStatusChanging(ServiceItem serviceItem);
        void ServiceStarted(ServiceItem serviceItem);
        void ServiceStopped(ServiceItem serviceItem);
        void ServiceRestarted(ServiceItem serviceItem);
        void ServiceError(ServiceItem serviceItem, String error);
        void ServiceStatusChangeProgress(ServiceItem serviceItem, long ticks);
        object PerformInvoke(Delegate method, params object[] args);
        void RefreshStatus(ServiceItem serviceItem);
    }

    [Serializable]
    public delegate void ServiceItemStatusEventHandler(ServiceItem sender);
    [Serializable]
    public delegate void ServiceItemErrorEventHandler(ServiceItem sender, String message);
    [Serializable]
    public delegate void ServiceItemProgressEventHandler(ServiceItem sender, long ticks);
    
    /// <summary>
    /// Wrapper for ServiceController
    /// </summary>
    public class ServiceItem
    {
        private List<ServiceItemListener> listeners;
        private List<ServiceGroupItem> groups;
        private ServiceController serviceController;
        private bool exists;
        private string serviceName;

        public ServiceItem(String serviceName)
        {
            this.serviceController = new ServiceController(serviceName);
            this.serviceName = serviceName;
            this.listeners = new List<ServiceItemListener>();
            this.groups = new List<ServiceGroupItem>();
            try
            {
                ServiceControllerStatus status = serviceController.Status;
                this.exists = true;
            }
            catch (Exception)
            {
                this.exists = false;
            }

        }

        public ServiceController Controller
        {
            get
            {
                return this.serviceController;
            }
        }

        public System.ServiceProcess.ServiceControllerStatus Status
        {
            get
            {
                return this.serviceController.Status;
            }
        }

        public String ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// List of objects which use this ServiceItem
        /// </summary>
        public List<ServiceItemListener> Listeners
        {
            get { return listeners; }
        }

        public List<ServiceGroupItem> Groups
        {
            get { return groups; }
        }

        public bool Exists
        {
            get { return exists; }
        }

        public void AddListener(ServiceItemListener listener)
        {
            listeners.Add(listener);
        }

        private bool changingState = false;

        public void Start()
        {
            changingState = true;
            NotifyStatusChangeStart();
            NotifyProgress();
            Thread thread = new Thread(new ThreadStart(this.ThreadDoStart));
            thread.Start();
           
        }

        public void Stop()
        {
            changingState = true;
            NotifyStatusChangeStart();
            NotifyProgress();
            Thread thread = new Thread(new ThreadStart(this.ThreadDoStop));
            thread.Start();
            
        }

        private void ThreadDoStart()
        {
            try
            {
                serviceController.Start();
                WaitForStatus(ServiceControllerStatus.Running);
                notifyStarted();
            }
            catch (Exception ex)
            {
                notifyError(ex.Message);
            }
            finally
            {
                changingState = false;
            }
        }

        private void ThreadDoStop()
        {
            try
            {
                serviceController.Stop();
                WaitForStatus(ServiceControllerStatus.Stopped);
                notifyStopped();
            }
            catch (Exception ex)
            {
                notifyError(ex.Message);
            }
            finally
            {
                changingState = false;
            }
        }

        private void notifyStarted()
        {
            foreach (ServiceItemListener listener in listeners)
            {
                ServiceItemStatusEventHandler callback =
                    new ServiceItemStatusEventHandler(listener.ServiceStarted);
                listener.PerformInvoke(callback, new Object[] { this });
            }
        }

        private void notifyStopped()
        {
            foreach (ServiceItemListener listener in listeners)
            {
                ServiceItemStatusEventHandler callback =
                    new ServiceItemStatusEventHandler(listener.ServiceStopped);
                listener.PerformInvoke(callback, new Object[] { this });
            }
        }

        private void notifyError(String errorMessage)
        {
            foreach (ServiceItemListener listener in listeners)
            {
                ServiceItemErrorEventHandler callback =
                    new ServiceItemErrorEventHandler(listener.ServiceError);
                listener.PerformInvoke(callback, new Object[] { this, errorMessage });
            }
        }

        private void NotifyStatusChangeStart(){
            foreach (ServiceItemListener listener in listeners)
            {
                ServiceItemStatusEventHandler callback =
                    new ServiceItemStatusEventHandler(listener.ServiceStatusChanging);
                listener.PerformInvoke(callback, new Object[] { this });
            }
        }

        private const float SECONDS_TO_WAIT_TO_STATUS = 30;
        private void WaitForStatus(ServiceControllerStatus desiredStatus)
        {
            serviceController.WaitForStatus(desiredStatus, TimeSpan.FromSeconds(SECONDS_TO_WAIT_TO_STATUS));
        }

        private void NotifyProgress()
        {
            Thread thread = new Thread(new ThreadStart(this.ThreadDoNotifyProgress));
            thread.Start();
        }

        private void ThreadDoNotifyProgress()
        {
            try
            {
                long startTicks = DateTime.Now.Ticks;
                long currentTicks = startTicks;
                while (changingState)
                {
                    foreach (ServiceItemListener listener in listeners)
                    {
                        ServiceItemProgressEventHandler callback =
                            new ServiceItemProgressEventHandler(listener.ServiceStatusChangeProgress);
                        listener.PerformInvoke(callback, new Object[] { this, currentTicks-startTicks });
                    }
                    Thread.Sleep(200);
                    currentTicks = DateTime.Now.Ticks;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error progress : " + ex.Message);
            }
            finally
            {

            }
        }
    }
}
