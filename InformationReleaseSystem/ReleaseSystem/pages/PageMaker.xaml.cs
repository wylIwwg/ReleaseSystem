
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReleaseSystem.bean;
using ReleaseSystem.fileserver;
using ReleaseSystem.ui;
using ReleaseSystem.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace ReleaseSystem.pages
{
    /// <summary>
    /// PageMaker.xaml 的交互逻辑
    /// </summary>
    public partial class PageMaker : Page
    {
        public PageMaker()
        {
            InitializeComponent();



        }

        //添加分辨率
        List<Ratio> ratios = new List<Ratio>();
        //添加控件类型
        List<ControlType> controlTypes = new List<ControlType>();

        double RATIO = 0.4;

        Rectangle currentRect;//当前选中的控件

        List<Rectangle> controlList = new List<Rectangle>();//页面制作的控件集合
        List<BeanControls> controls = new List<BeanControls>();//保存的实体类控件
        string controlType;//控件类型

        //添加控件到canvas
        private void btnAddControl_Click(object sender, RoutedEventArgs e)
        {
            if (currentRect != null)
            {
                ShowUtils.showTips(false, "有未保存的控件？");
                return;
            }

            VisualBrush visual = new VisualBrush();
            Rectangle thumb = new Rectangle();

            thumb.Height = 100 * RATIO;
            thumb.Width = 200 * RATIO;
            thumb.Stroke = Brushes.Red;
            thumb.MouseDown += thumb_MouseDown;
            thumb.StrokeThickness = 1;

            CtrTag tag = new CtrTag();

            if ("ImageView".Equals(controlType))
            {
                Image image = new Image();
                visual.Visual = image;
                tag.type = "ImageView";

            }
            else
            {

                TextBlock tb = new TextBlock();
                tb.Text = "点击编辑文字内容";
                tb.FontSize = 10;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                tb.Width = thumb.Width;
                tb.Height = thumb.Height;

                visual.Visual = tb;
                tbContent.Text = tb.Text;

                tag.type = "TextView";


            }

            DragControlHelper.SetIsSelectable(thumb, true);
            DragControlHelper.SetIsEditable(thumb, true);

            visual.Stretch = Stretch.Uniform;
            thumb.Fill = visual;
            controlList.Add(thumb);
            canvas.Children.Add(thumb);
            thumb.Tag = tag;
            currentRect = thumb;

        }


        private T SearchVisualTree<T>(DependencyObject tarElem) where T : DependencyObject
        {
            if (tarElem != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(tarElem);
                if (count == 0)
                    return null;
                for (int i = 0; i < count; ++i)
                {
                    var child = VisualTreeHelper.GetChild(tarElem, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    else
                    {
                        var res = SearchVisualTree<T>(child);
                        if (res != null)
                        {
                            return res;
                        }
                    }
                }
                return null;
            }
            return null;
        }

        //重置选中的控件，坐标信息
        void thumb_MouseDown(object sender, MouseButtonEventArgs e)
        {

            currentRect = sender as Rectangle;
            Rect NewBound = Rect.Empty;
            CtrTag tag = (CtrTag)currentRect.Tag;

            if (tag != null)
            {
                NewBound = tag.rect;
            }

            if ("ImageView".Equals(tag.type))
            {
                spImage.Visibility = Visibility.Visible;
                spText.Visibility = Visibility.Collapsed;
            }
            else
            {
                spImage.Visibility = Visibility.Collapsed;
                spText.Visibility = Visibility.Visible;
            }


            if (NewBound != Rect.Empty)
            {
                tbWorkHeight.Text = NewBound.Height / RATIO + "";
                tbWorkWidth.Text = NewBound.Width / RATIO + "";
                Point p = NewBound.Location;
                tbX.Text = p.X / RATIO + "";
                tbY.Text = p.Y / RATIO + "";

                VisualBrush vb = (VisualBrush)currentRect.Fill;
                if ("ImageView".Equals(tag.type))
                {

                }
                else
                {
                    TextBlock tb = (TextBlock)vb.Visual;

                    tbContent.Text = tb.Text;
                }



            }
            else
            {
                tbWorkHeight.Text = currentRect.Height / RATIO + "";
                tbWorkWidth.Text = currentRect.Width / RATIO + "";
                tbX.Text = "0";
                tbY.Text = "0";


            }

        }



        //控件拖动完成 更新坐标信息
        void helper_DragCompleted(object Sender, DragChangedEventArgs e)
        {

            currentRect = (Rectangle)e.DragTargetElement;
            tbWorkHeight.Text = e.NewBound.Height / RATIO + "";
            tbWorkWidth.Text = e.NewBound.Width / RATIO + "";
            Point p = e.NewBound.Location;
            tbX.Text = p.X / RATIO + "";
            tbY.Text = p.Y / RATIO + "";
            CtrTag tag = (CtrTag)currentRect.Tag;
            tag.rect = e.NewBound;
            currentRect.Tag = tag;

        }


        private void tbWorkWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (currentRect != null)
            {
                Regex re = new Regex("[^0-9.-]+");
                bool result = re.IsMatch(e.Text);
                e.Handled = result;
            }


        }

        private void tbWorkHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (currentRect != null)
            {
                Regex re = new Regex("[^0-9.-]+");
                bool result = re.IsMatch(e.Text);
                e.Handled = result;
            }
        }

        private void tbWorkWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentRect != null)
            {
                try
                {

                    int width = Int32.Parse(tbWorkWidth.Text);
                    if (width > 0 && currentRect != null)
                    {
                        currentRect.Width = width * RATIO;
                        VisualBrush vb = (VisualBrush)currentRect.Fill;
                        // StackPanel sp = (StackPanel)vb.Visual;

                        //  TextBlock tb = SearchVisualTree<TextBlock>(sp);
                        TextBlock tb = (TextBlock)vb.Visual;
                        tb.Text = tbContent.Text;
                        currentRect.Fill = vb;
                    }


                }
                catch (Exception error)
                {

                }
            }
        }

        private void tbWorkHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentRect != null)
            {
                try
                {
                    int height = Int32.Parse(tbWorkHeight.Text);
                    if (height > 0)
                    {
                        currentRect.Height = height * RATIO;

                        VisualBrush vb = (VisualBrush)currentRect.Fill;
                        // StackPanel sp = (StackPanel)vb.Visual;

                        //TextBlock tb = SearchVisualTree<TextBlock>(sp);
                        TextBlock tb = (TextBlock)vb.Visual;

                        tb.Text = tbContent.Text;
                        currentRect.Fill = vb;

                    }
                    //borderwork.Height = height;

                }
                catch (Exception error)
                {
                }
            }

        }

        private void cbRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ratio ratio = (Ratio)cbRatio.SelectedValue;
            //960*540  540*960  
            // 576*324  324*576

            if (borderwork != null && ratio != null)
            {

                borderwork.Width = ratio.width * RATIO + 2;
                borderwork.Height = ratio.heigth * RATIO + 2;

            }

        }



        private void cbBgColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TextBlock tb = cbBgColor.SelectedValue as TextBlock;
            if (tb != null)
            {
                canvas.Background = tb.Background;
            }
        }



        //设备背景图片
        System.Windows.Forms.OpenFileDialog openFileDialog;
        private void btnBgImage_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "|*.jpg;*.png";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做获取文件路径 ...=openFileDialog1.FileName; 
                string imgPath = openFileDialog.FileName;
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(imgPath));
                b.Stretch = Stretch.Fill;
                canvas.Background = b;
            }
        }


        private void pageMaker_Loaded(object sender, RoutedEventArgs e)
        {


            ratios.Add(new Ratio(2, 1080, 1920));
            ratios.Add(new Ratio(1, 1920, 1080));

            cbRatio.ItemsSource = ratios;

            cbRatio.SelectedIndex = 0;

            cbBgColor.SelectedIndex = 0;


            controlTypes.Add(new ControlType("TextView"));
            controlTypes.Add(new ControlType("ImageView"));
            cbControlType.ItemsSource = controlTypes;
            cbControlType.SelectedIndex = 0;

            var lastdir = ConfigHelper.GetWorkDir();
            if (Directory.Exists(lastdir))
                VM.WorkDir = lastdir;
        }

        //删除控件
        private void btnDelControl_Click(object sender, RoutedEventArgs e)
        {
            if (currentRect != null)
            {
                canvas.Children.Remove(currentRect);
                controlList.Remove(currentRect);
                currentRect = null;
                tbWorkHeight.Text = "0";
                tbWorkWidth.Text = "0";
                tbX.Text = "0";
                tbY.Text = "0";
            }
        }





        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (controls.Count > 0)
            {

                string resu = JsonConvert.SerializeObject(controls);
                Console.WriteLine(resu);
                JObject jo = new JObject();
                jo.Add("data", resu);
                jo.Add("type", "custom");
                WMain w = (WMain)Window.GetWindow(this);

                if (w.ClientProxSocketList.Count > 0)
                {
                    foreach (Socket proxSocket in w.ClientProxSocketList)
                    {
                        if (proxSocket.Connected)
                        {
                            //原始的字符串转成的字节数组。
                            // byte[] data = Encoding.Default.GetBytes(txtMsg.Text);
                            byte[] data = Encoding.UTF8.GetBytes(jo.ToString() + "\n");

                            proxSocket.Send(data, 0, data.Length, SocketFlags.None);
                        }

                    }


                }


                //发送完 清除
                // canvas.Children.Clear();
                foreach (Rectangle rex in controlList)
                {
                    canvas.Children.Remove(rex);

                }
                controls.Clear();
                controlList.Clear();
                currentRect = null;
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/control.txt");




            }
        }





        private CancellationTokenSource _cancelToken;
        private HttpFileServer _fileSvr;
        private string curexefile;
        private HttpListener Listener;
        private ViewModel VM;


        private void CbControlType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlType type = (ControlType)cbControlType.SelectedValue;
            controlType = type.value;
            Console.WriteLine("当前控件: " + controlType);
            if ("ImageView".Equals(controlType))
            {
                spImage.Visibility = Visibility.Visible;
                spText.Visibility = Visibility.Collapsed;
            }
            else
            {
                spImage.Visibility = Visibility.Collapsed;
                spText.Visibility = Visibility.Visible;
            }
        }

        private void BtnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            var browser = new System.Windows.Forms.FolderBrowserDialog();
            browser.RootFolder = Environment.SpecialFolder.MyComputer;

            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                VM.WorkDir = browser.SelectedPath;
                ConfigHelper.SaveWorkDir(VM.WorkDir);
                Stop();
                Run();
            }

        }

        #region Methods

        private async void AckRequest(HttpListenerContext context)
        {
            var request = context.Request;
            if (request.RawUrl.EndsWith("favicon.ico"))
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                return;
            }
            var model = new RequestModel(request.RawUrl, request.RemoteEndPoint);
            await Dispatcher.InvokeAsync(() =>
            {
                VM.RequestModels.Add(model);
            });
            var dstpath = System.IO.Path.Combine(VM.WorkDir, request.Url.LocalPath.TrimStart('/'));
            dstpath = dstpath.Replace('/', '\\').TrimEnd('\\');
            var targetTag = request.Headers["If-None-Match"];
            var srcTag = _fileSvr.GetPathETag(dstpath);

            if (!string.IsNullOrWhiteSpace(targetTag) && targetTag == srcTag)
            {
                context.Response.StatusCode = 304;
            }
            else
            {//etag不匹配 重新设置etag和内容
                context.Response.AppendHeader("Cache-Control", "no-cache");
                context.Response.AppendHeader("Etag", srcTag);

                var html = _fileSvr.GetDirContentHtmlText(dstpath, dstpath.Equals(VM.WorkDir.TrimEnd('\\')));

                //是否为文件夹
                if (!string.IsNullOrWhiteSpace(html))
                {
                    var data = Encoding.UTF8.GetBytes(html);
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.ContentType = "text/html";
                    context.Response.OutputStream.Write(data, 0, data.Length);
                }
                else
                {//文件 或 目标不存在
                    if (File.Exists(dstpath))
                    {
                        using (var fs = new FileStream(dstpath, FileMode.Open, FileAccess.Read))
                        {
                            model.Status = "Acking";
                            context.Response.ContentLength64 = fs.Length;
                            try
                            {
                                await fs.CopyToAsync(context.Response.OutputStream);
                                context.Response.OutputStream.Flush();
                            }
                            catch { }
                        }
                    }
                    else
                    {//文件不存在
                        var filename = System.IO.Path.GetFileName(dstpath);
                        if (filename.Equals("{HttpFileServer}"))
                        {
                            context.Response.AppendHeader("Cache-Control", "No-cache");
                            context.Response.AppendHeader("Content-Disposition", "attachment;filename=HttpFileServer.exe");
                            using (var fs = new FileStream(curexefile, FileMode.Open, FileAccess.Read))
                            {
                                model.Status = "Acking";
                                context.Response.ContentLength64 = fs.Length;
                                await fs.CopyToAsync(context.Response.OutputStream);
                                context.Response.OutputStream.Flush();
                            }
                        }
                        else
                            context.Response.StatusCode = 404;
                    }
                }
            }
            context.Response.Close();
            model.Status = "Done";
            Dispatcher.Invoke(() =>
            {
                Console.WriteLine(model.ToString() + "\t... " + context.Response.StatusCode);
                VM.RequestModels.Remove(model);
            });
        }

        private void Run()
        {
            var path = $"http://+:{VM.ListenPort}/";
            Listener.Prefixes.Clear();
            try
            {
                Listener.Prefixes.Add(path);
                Listener.Start();
                _fileSvr.Start(VM.WorkDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Listener = new HttpListener();
                return;
            }
            _cancelToken = new CancellationTokenSource();
            Console.WriteLine($"Start Listening :");
            foreach (var p in Listener.Prefixes)
            {
                Console.WriteLine(p);
            }
            var ips = IPHelper.GetAllLocalIP();

            Task.Factory.StartNew(() =>
            {
                DoListen();
            }, _cancelToken.Token);
            foreach (var ip in ips)
            {
                var str = $"http://{ip}:{VM.ListenPort}/";
                Console.WriteLine(str);
            }

            tbWorkDir.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
        }


        public void Stop()
        {
            if (_cancelToken != null)
                _cancelToken.Cancel(true);
            if (Listener != null)
                Listener.Stop();
            if (_fileSvr != null)
                _fileSvr.Stop();
            tbWorkDir.Background = new SolidColorBrush();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _cancelToken.Cancel(true);
            Listener.Stop();
            _fileSvr.Stop();

            tbWorkDir.Background = new SolidColorBrush();
        }

        private void DoListen()
        {
            while (!_cancelToken.IsCancellationRequested)
            {
                try
                {
                    var context = Listener.GetContext();
                    Task.Run(() => AckRequest(context));
                }
                catch (Exception)
                {
                }
            }
        }


        private string ToHtmlLinkString(string localpath, string rootpath, string endstr = "")
        {
            var name = localpath.Replace(rootpath.TrimEnd('\\') + "\\", "").Replace("\\", "/");

            return $"<div><a href=\"./{name}{endstr}\">{name}</a></div>";
        }

        #endregion Methods



        /// <summary>
        /// 
        /// 选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImaChoose_Click(object sender, RoutedEventArgs e)
        {
            if (currentRect != null)
            {
                openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.InitialDirectory = VM.WorkDir;

                // openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "|*.jpg;*.png";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //此处做获取文件路径 ...=openFileDialog1.FileName; 
                    string imgPath = openFileDialog.FileName;
                    Console.WriteLine(imgPath);
                    imgThumb.Source = new BitmapImage(new Uri(imgPath));

                    VisualBrush vb = (VisualBrush)currentRect.Fill;
                    Image img = (Image)vb.Visual;
                    Console.WriteLine(imgPath.Substring(imgPath.LastIndexOf("\\") + 1));
                    img.Tag = imgPath.Substring(imgPath.LastIndexOf("\\") + 1);
                    img.Source = new BitmapImage(new Uri(imgPath));
                    img.Stretch = Stretch.Uniform;


                }

            }

        }




        /// <summary>
        /// 保存当前修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void BtnSaveCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (currentRect != null)
            {
                CtrTag tag = (CtrTag)currentRect.Tag;
                BeanControls bc = new BeanControls();
                ControlAttribute attribute = new ControlAttribute();


                Rect NewBound = Rect.Empty;
                if (tag != null)
                {
                    NewBound = (Rect)tag.rect;
                }
                if (NewBound != Rect.Empty)
                {
                    Point p = NewBound.Location;
                    bc.X = p.X / RATIO;
                    bc.Y = p.Y / RATIO;
                    bc.Width = NewBound.Width / RATIO;
                    bc.Height = NewBound.Height / RATIO;


                }

                if ("TextView".Equals(tag.type))
                {
                    VisualBrush vb = (VisualBrush)currentRect.Fill;
                    // StackPanel sp = (StackPanel)vb.Visual;

                    //TextBlock tb = SearchVisualTree<TextBlock>(sp);
                    TextBlock tb = (TextBlock)vb.Visual;
                    string txt = tb.Text;

                    bc.control = "TextView";
                    attribute.text = txt;

                    bc.attribute = attribute;

                }
                else
                {
                    VisualBrush vb = (VisualBrush)currentRect.Fill;
                    Image img = (Image)vb.Visual;
                    string path = "http://192.168.2.188:80/" + (string)img.Tag;
                    attribute.backgroundImage = path;
                    bc.attribute = attribute;
                    bc.control = "ImageView";
                }

                controls.Add(bc);
                currentRect = null;
                string resu = JsonConvert.SerializeObject(controls);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/control.txt", resu);
            }
        }

        //TextView文本内容修改
        private void TbContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentRect != null)
            {
                VisualBrush vb = (VisualBrush)currentRect.Fill;
                //StackPanel sp = (StackPanel)vb.Visual;

                //TextBlock tb = SearchVisualTree<TextBlock>(sp);
                TextBlock tb = (TextBlock)vb.Visual;
                tb.Width = currentRect.Width;
                tb.Height = currentRect.Height;
                tb.Text = tbContent.Text;
                if (tbFontSize.Text.Length > 0)
                {

                    tb.FontSize = Double.Parse(tbFontSize.Text);
                }
                vb.Visual = tb;
                currentRect.Fill = vb;
            }

        }
        private void TbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (currentRect != null)
                {
                    VisualBrush vb = (VisualBrush)currentRect.Fill;
                    // StackPanel sp = (StackPanel)vb.Visual;
                    //  TextBlock tb = SearchVisualTree<TextBlock>(sp);
                    TextBlock tb = (TextBlock)vb.Visual;
                    tb.Width = currentRect.Width;
                    tb.Height = currentRect.Height;
                    tb.Text = tbContent.Text;
                    if (tbFontSize.Text.Length > 0)
                    {
                        tb.FontSize = Double.Parse(tbFontSize.Text);
                    }
                    vb.Visual = tb;
                    currentRect.Fill = vb;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());

            }

        }
        DragChangedEventHandler del;
        private void PageMaker_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Console.WriteLine(e.NewValue);

                del = helper_DragCompleted;

                helper.DragCompleted += del;

                VM = new ViewModel();
                DataContext = VM;
                Listener = new HttpListener();
                _fileSvr = new HttpFileServer();
            }
            else
            {
                helper.DragCompleted -= del;
                if (currentRect != null)
                {
                    ShowUtils.showTips(false, "未保存！");
                }
            }
        }
    }
}
