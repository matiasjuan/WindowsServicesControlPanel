
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DemoServices.UI.Common;

namespace DemoServices.UI.WPF
{
    /// <summary>
    /// Interaction logic for UIExpander.xaml
    /// </summary>
    public partial class UIExpander : UserControl
    {
        public UIExpander()
        {
            InitializeComponent();
        }

        internal void LoadGroup(ServiceGroupItem g)
        {
            //theExpander.Header = g.Key;
            lblTitle.Content = g.Key;
            lblTotal.Content = g.TotalServices;
            lblRunning.Content = g.RunningServices;

            theExpander.IsExpanded = false;

            foreach (var item in g.Services)
            {
                if (item.Exists)
                {
                    UIServiceItem si = new UIServiceItem();
                    si.ServiceItem = item;

                    stackPanel.Children.Add(si);
                }
                else
                {
                    UIInvalidService si = new UIInvalidService();
                    si.ServiceName = item.ServiceName;
                    stackPanel.Children.Add(si);
                }
            }

        }

       
    }
}
