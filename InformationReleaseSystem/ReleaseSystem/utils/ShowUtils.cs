using ReleaseSystem.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ReleaseSystem.utils
{
    public class ShowUtils
    {
        static Window tipsWindow;
        static SynchronizationContext mContext;
        internal static void init(SynchronizationContext context)
        {
            mContext = context;
        }

        public static void showTips(Window owner, bool yes, string tips, TipsControl.ConfirmLisiter e)
        {
            tipsWindow = new Window();
            TipsControl tw = new TipsControl(tips, 300, 300);

            tipsWindow.Height = 300;
            tw.border.BorderBrush = new SolidColorBrush(yes ? Colors.Green : Colors.Red);
            tipsWindow.Width = 300;
            tw.window = tipsWindow;
            tw.imgError.Visibility = yes ? Visibility.Hidden : Visibility.Visible;
            tw.imgSuccess.Visibility = !yes ? Visibility.Hidden : Visibility.Visible;
            tw.btnConfirmClick += e;
            tw.btnCancel.Visibility = Visibility.Collapsed;//取消按钮
            tipsWindow.Owner = owner;
            tipsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tipsWindow.WindowStyle = WindowStyle.None;
            tipsWindow.Opacity = 1;
            tipsWindow.AllowsTransparency = true;
            tipsWindow.Background = new SolidColorBrush(Colors.Transparent);
            tipsWindow.Content = tw;
            tipsWindow.Title = "提示";
            tipsWindow.Topmost = true;

            tipsWindow.ShowDialog();
        }



        public static void showTips(Window owner, bool yes, string tips)
        {
            showTips(owner, yes, tips, null);

        }



        public static void showTips(bool yes, string tips)
        {

            mContext.Post(new SendOrPostCallback(showTips), tips);


        }

        public static void showTips(object tips)
        {

            mContext.Post(new SendOrPostCallback(closeLoading), null);

            tipsWindow = new Window();
            TipsControl tw = new TipsControl(tips.ToString(), 300, 300);

            tipsWindow.Height = 300;
            tipsWindow.Topmost = true;
            tw.border.BorderBrush = new SolidColorBrush(Colors.Red);
            tipsWindow.Width = 300;
            tw.window = tipsWindow;
            tw.imgError.Visibility = Visibility.Visible;
            tw.imgSuccess.Visibility = Visibility.Hidden;

            tw.btnCancel.Visibility = Visibility.Collapsed;//取消按钮
            tipsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tipsWindow.WindowStyle = WindowStyle.None;
            tipsWindow.Opacity = 1;
            tipsWindow.AllowsTransparency = true;
            tipsWindow.Background = new SolidColorBrush(Colors.Transparent);
            tipsWindow.Content = tw;
            tipsWindow.Title = "提示";

            //tipsWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/ico_tips.ico"));
            tipsWindow.ShowDialog();
        }



        static Window loadingWindow;


        public static void closeLoading()
        {

            mContext.Post(new SendOrPostCallback(closeLoading), null);
        }

        public static void closeLoading(object load)
        {
            if (loadingWindow != null)
            {
                loadingWindow.Close();
            }

        }


        public static void showLoading(string load)
        {

            mContext.Post(new SendOrPostCallback(showLoading), load);
        }


        public static void showLoading(object load)
        {

            loadingWindow = new Window();
            LoadingControl lc = new LoadingControl();
            loadingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            loadingWindow.WindowStyle = WindowStyle.None;
            loadingWindow.Width = 200;
            loadingWindow.Height = 200;
            loadingWindow.Opacity = 1;
            lc.loading.Content = load.ToString();
            loadingWindow.AllowsTransparency = true;
            loadingWindow.Background = new SolidColorBrush(Colors.Transparent);
            loadingWindow.Content = lc;
            loadingWindow.Topmost = true;
            loadingWindow.ShowDialog();
        }

    }
}
