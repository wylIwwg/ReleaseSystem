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
    /// PageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PageControl : UserControl
    {
        public class PageClickRoutedEventArgs : RoutedEventArgs
        {
            public PageClickRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

            public string page { get; set; }
        }



        static PageControl source;
        static int currPage;
        static int pages;

        static List<RadioButton> rbList = new List<RadioButton>();

        public PageControl()
        {
            InitializeComponent();
            source = this;

        }







        /// <summary>
        /// pages
        /// </summary>
        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(int),
             typeof(PageControl),
             new PropertyMetadata(0, new PropertyChangedCallback(OnPagesChanged)));

        public int Pages
        {
            get { return (int)GetValue(PagesProperty); }

            set
            {
                SetValue(PagesProperty, value);
            }
        }

        void OnPagesChanged(int value)
        {
            if (pages > 0)
            {
                source.spPages.Children.Clear();
                rbList.Clear();
                for (int i = 1; i <= pages; i++)
                {
                    RadioButton rb = new RadioButton();
                    rb.Content = i + "";
                    Style myStyle = (Style)source.FindResource("rbPages");//cb 这个样式是引用的资源文件中的样式名称
                    rb.Style = myStyle;
                    rb.Checked += rb_Checked;
                    rbList.Add(rb);
                    currPage = 1;
                    if (i == 1)
                    {
                        rb.IsChecked = true;
                    }
                    source.spPages.Children.Add(rb);
                }
            }

        }

        static void OnPagesChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            PageControl source = (PageControl)sender;
            pages = (int)args.NewValue;
            if (pages > 0)
            {
                source.spPages.Children.Clear();
                rbList.Clear();
                for (int i = 1; i <= pages; i++)
                {
                    RadioButton rb = new RadioButton();
                    rb.Content = i + "";
                    Style myStyle = (Style)source.FindResource("rbPages");//cb 这个样式是引用的资源文件中的样式名称
                    rb.Style = myStyle;
                    rb.Checked += rb_Checked;
                    rbList.Add(rb);
                    currPage = 1;
                    if (i == 1)
                    {
                        rb.IsChecked = true;
                    }
                    source.spPages.Children.Add(rb);
                }
            }

        }



        static void rb_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            currPage = Int32.Parse(rb.Content.ToString());

            if (pagesDelegates != null)
            {
                Console.WriteLine("rb_Checked  " + pagesDelegates.Count);
                foreach (PageChangedDelegate pcd in pagesDelegates)
                {
                    if (pcd != null)
                    {
                        //source  pcd.pageChangedEvent(currPage);
                        pcd(currPage);
                    }
                }
            }



        }



        /// <summary>
        /// pages
        /// </summary>
        public static readonly DependencyProperty PageChangedDelegateProperty = DependencyProperty.Register("PagesDelegate", typeof(PageChangedDelegate),
             typeof(PageControl),
             new PropertyMetadata(null, new PropertyChangedCallback(OnPagesDelegateChanged)));

        public PageChangedDelegate PagesDelegate
        {
            get { return (PageChangedDelegate)GetValue(PageChangedDelegateProperty); }

            set
            {
                SetValue(PageChangedDelegateProperty, value);
            }
        }
        static void OnPagesDelegateChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            PageControl source = (PageControl)sender;
            if (pagesDelegates == null)
            {
                pagesDelegates = new List<PageChangedDelegate>();
            }
            pagesDelegates.Add((PageChangedDelegate)args.NewValue);
        }


        static List<PageChangedDelegate> pagesDelegates;
        //声明委托 和 事件

        public delegate void PageChangedDelegate(int page);
        /* public delegate void PageChangedDelegate1(int page);
         public delegate void PageChangedDelegate2(int page);
         public PageChangedDelegate1 pageChangedDelegate1;
         public PageChangedDelegate1 pageChangedDelegate2;
         */
        public event PageChangedDelegate pageChangedEvent;







        #region
        //声明和注册路由事件
        public static readonly RoutedEvent LastClickEvent =
            EventManager.RegisterRoutedEvent("LastPageClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(PageControl));
        //CLR事件包装
        public event RoutedEventHandler LastPageClick
        {
            add { this.AddHandler(LastClickEvent, value); }
            remove { this.RemoveHandler(LastClickEvent, value); }
        }

        //激发路由事件,借用Click事件的激发方法

        public void OnLastPageClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(LastClickEvent, this);
            this.RaiseEvent(args);
        }

        #endregion



        #region
        //声明和注册路由事件
        public static readonly RoutedEvent NextClickEvent =
            EventManager.RegisterRoutedEvent("NextPageClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(PageControl));
        //CLR事件包装
        public event RoutedEventHandler NextPageClick
        {
            add { this.AddHandler(NextClickEvent, value); }
            remove { this.RemoveHandler(NextClickEvent, value); }
        }

        //激发路由事件,借用Click事件的激发方法

        public void OnNextPageClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(NextClickEvent, this);
            this.RaiseEvent(args);
        }

        #endregion




        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (currPage - 1 > 0)
            {

                OnLastPageClick();
                currPage -= 1;
                changedState();
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currPage + 1 <= pages)
            {

                OnNextPageClick();
                currPage += 1;
                changedState();



            }

        }

        private void changedState()
        {
            Console.WriteLine("changedState  " + pagesDelegates.Count);
            foreach (RadioButton rb in rbList)
            {
                if (rb.Content.ToString().Equals(currPage.ToString()))
                {
                    rb.IsChecked = true;
                    if (pageChangedEvent != null)
                    {
                        pageChangedEvent(currPage);
                    }


                }
            }
        }
    }
}
