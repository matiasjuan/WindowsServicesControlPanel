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

namespace DemoServices.UI.WPF
{
    /// <summary>
    /// Interaction logic for UIInvalidService.xaml
    /// </summary>
    public partial class UIInvalidService : UserControl
    {
        public UIInvalidService()
        {
            InitializeComponent();
        }
        public String ServiceName
        {
            get
            {
                return lbl.Content.ToString();
            }
            set
            {

                lbl.Content = value;

            }
        }
    }
}
