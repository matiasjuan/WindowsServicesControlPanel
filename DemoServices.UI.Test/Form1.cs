using DemoServices.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoServices.UI.Test
{
    public partial class Form1 : Form, ServiceItemListener
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();

            si = new ServiceItem("Tomcat");
            si.AddListener(this);
        }

        private ServiceItem si;

        private void button1_Click(object sender, EventArgs e)
        {
            si.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            si.Stop();
        }

        public object PerformInvoke(Delegate method, params object[] args)
        {
            return this.Invoke(method, args);
        }

        public void ServiceStarted(ServiceItem serviceItem)
        {
            addMessage(String.Format("service item {0} started", serviceItem.ServiceName));
        }

        public void ServiceStopped(ServiceItem serviceItem)
        {
            addMessage(String.Format("service item {0} stopped", serviceItem.ServiceName));
        }

        public void ServiceRestarted(ServiceItem serviceItem)
        {
            throw new NotImplementedException();
        }

        public void ServiceError(ServiceItem serviceItem, string error)
        {
            addMessage(String.Format("service item {0} error {1}", serviceItem.ServiceName, error));
        }

        private void addMessage(String message)
        {
            listBox1.Items.Add(message);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        public void ServiceStatusChangeProgress(ServiceItem serviceItem, long ticks)
        {
            TimeSpan ts = new TimeSpan(ticks);
            label1.Text = ts.TotalMilliseconds.ToString();
        }

        public void ServiceStatusChanging(ServiceItem serviceItem)
        {
            throw new NotImplementedException();
        }

        public void RefreshStatus(ServiceItem serviceItem)
        {
            throw new NotImplementedException();
        }
    }
}
