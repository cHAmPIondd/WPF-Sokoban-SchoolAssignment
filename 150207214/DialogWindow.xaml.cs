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
using System.Windows.Shapes;

namespace _150207214
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : Window
    {
        private string m_RequestCode;
        public DialogWindow(string requestCode,string label)
        {
            InitializeComponent();
            m_RequestCode = requestCode;
            content_label.Content = label;
        }

        private void done_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MapEditorWindow)Owner).DialogCallback(m_RequestCode, content_text.Text);
                Close();
            }
            catch
            {

            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Owner.Visibility = System.Windows.Visibility.Visible;
            Owner.Show();
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                done_button_Click(null,null);
            }
        }
    }
}
