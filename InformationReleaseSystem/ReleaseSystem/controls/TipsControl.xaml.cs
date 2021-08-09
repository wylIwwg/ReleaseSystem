using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReleaseSystem.controls
{
    /// <summary>
    /// TipsControl.xaml 的交互逻辑
    /// </summary>
    public partial class TipsControl : UserControl
    {
        public Window window;

        public TipsControl(string tips)
        {
            InitializeComponent();
            lbTips.Text = tips;

        }
        public TipsControl(string tips, int width, int height)
        {
            InitializeComponent();
            lbTips.Text = tips;
            this.Width = width; this.Height = height;


        }
        public delegate void ConfirmLisiter();
        public event ConfirmLisiter btnConfirmClick;

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (btnConfirmClick != null)
            {
                btnConfirmClick();
            }
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
