using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DemoServices.UI.Common
{
    public class ServiceGroups {
        private Dictionary<string, ServiceGroupItem> groupItems;
        private Dictionary<string, ServiceItem> services;

        public ServiceGroups()
        {
            this.groupItems = new Dictionary<string,ServiceGroupItem>();
            this.services = new Dictionary<string, ServiceItem>(); 
        }

        public void LoadFromFile(String fileName)
        {
            /*
             Add("group1", "Tomcat");
             Add("group1", "MuleMSMQService");
             Add("group2", "MuleMSMQService");
             Add("group3", "Tomcat");
            */


            //Load xml
            XDocument xdoc = XDocument.Load(fileName);

            //Run query
            var lv1s = from lv1 in xdoc.Descendants("group")
                       select new
                       {
                           Header = lv1.Attribute("name").Value,
                           Children = lv1.Descendants("service")
                       };

            //Loop through results
            foreach (var lv1 in lv1s)
            {
                foreach (var lv2 in lv1.Children)
                {
                    String group = lv1.Header;
                    String serviceName = lv2.Attribute("name").Value;

                    
                    Add(group, serviceName);
                }
            }
        }


        private void Add(string groupName, string serviceName)
        {
            ServiceItem si = null;
            if (services.ContainsKey(serviceName))
            {
                si = services[serviceName];
            }
            else
            {
                si = new ServiceItem(serviceName);
                services.Add(serviceName, si);
            }

            ServiceGroupItem gi = null;
            if (!groupItems.ContainsKey(groupName))
            {
                gi = new ServiceGroupItem(groupName);
                groupItems.Add(gi.Key, gi);
            }
            else
            {
                gi = groupItems[groupName];
            }

            gi.AddServiceItem(si);
        }

        public Dictionary<string, ServiceGroupItem> Groups
        {
            get { return groupItems; }
        }

        public Dictionary<string, ServiceItem> ServiceItems
        {
            get { return services; }
        }

        public ServiceItem FindServiceByKey(string serviceName)
        {
            if (services.ContainsKey(serviceName))
            {
                return services[serviceName];
            }
            return null;
        }

    }

    public class ServiceGroupItem
    {
        private Dictionary<string, ServiceItem> serviceItems;
        private String key;

        public ServiceGroupItem(String key)
        {
            this.key = key;
            serviceItems = new Dictionary<string, ServiceItem>();
        }

        public String Key
        {
            get { return key; }
        }

        public void AddServiceItem(ServiceItem service)
        {
            if (serviceItems.ContainsKey(service.ServiceName))
            {
                throw new ArgumentException("Service item already exists");
            }
            this.serviceItems.Add(service.ServiceName, service);
            service.Groups.Add(this);
        }

        public List<ServiceItem> Services
        {
            get { return serviceItems.Values.ToList<ServiceItem>();  }
        }

        public ServiceItem FindServiceItem(String key)
        {
            if (!serviceItems.ContainsKey(key))
                return null;
            return serviceItems[key];
        }

        public int RunningServices
        {
            get
            {
                int total = 0;
                foreach(var s in serviceItems)
                {
                    if (s.Value.Exists && s.Value.Status == ServiceControllerStatus.Running)
                        total++;
                }
                return total;
            }
        }
        public int TotalServices
        {
            get
            {
                int total = 0;
                foreach (var s in serviceItems)
                {
                    if (s.Value.Exists)
                        total++;
                }
                return total;
            }
        }
    }

    
}
