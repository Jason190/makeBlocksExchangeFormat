using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;
using System.Xml.Linq;

namespace makeBlocksExchangeFormat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int positiveNumb = 0;//记录有效点
        List<POSone> POSlist;//记录POS集合
        bool includePosture = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox2.Text = Properties.Settings.Default.picLong;
            textBox3.Text = Properties.Settings.Default.picWidth;
            textBox4.Text = Properties.Settings.Default.sensorLong;
            try { pixSize = int.Parse(Properties.Settings.Default.pixSize); } catch { }
            textBox5.Text = Properties.Settings.Default.focalLength;

            comboBox1.SelectedIndex = 0;

            this.Title += "    v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        void loadImage(ListBox inputListbox, string path, TextBox textBox)
        {
            tb_status.Text = "加载图片中";
            foreach (string file in Directory.GetFiles(path)) //循环里面套线程，有点危险
            {
                string file1 = file;//必须有,不妨试试不加这段代码，看看效果就明白了
                //起线程做
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
                {

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(file1, UriKind.RelativeOrAbsolute);
                    bitmap.DecodePixelWidth = 100;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    double ww = bitmap.PixelWidth;
                    double hh = bitmap.PixelHeight;
                    Image image = new Image();
                    image.Source = bitmap;
                    image.Height = 100;
                    //image.Width = 100 * (ww / hh);

                    inputListbox.Items.Add(image);
                    //tb_status.Text = "加载完毕";
                }));
                textBox.Text = path;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var i = list.Items[0];
        }
        //选择的照片变化时
        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_status.Text = "就绪";
            switch (tabcontrol1.SelectedIndex) //判断当前使用的tab页
            {
                case 0:
                    if (listBox1.SelectedItems.Count != 0 && listBox1.SelectedIndex == 0) //判断是否有选中的项
                    {
                        btnSetFrist.IsEnabled = true;
                    }
                    else if (listBox1.SelectedItems.Count == 0 && listBox1.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = false;
                    }
                    break;
                case 1:
                    if (listBox2.SelectedItems.Count != 0 && listBox2.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = true;
                    }
                    else if (listBox2.SelectedItems.Count == 0 && listBox2.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = false;
                    }
                    break;
                case 2:
                    if (listBox3.SelectedItems.Count != 0 && listBox3.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = true;
                    }
                    else if (listBox3.SelectedItems.Count == 0 && listBox3.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = false;
                    }
                    break;
                case 3:
                    if (listBox4.SelectedItems.Count != 0 && listBox4.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = true;
                    }
                    else if (listBox4.SelectedItems.Count == 0 && listBox4.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = false;
                    }
                    break;
                case 4:
                    if (listBox5.SelectedItems.Count != 0 && listBox5.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = true;
                    }
                    else if (listBox5.SelectedItems.Count == 0 && listBox5.SelectedIndex == 0)
                    {
                        btnSetFrist.IsEnabled = false;
                    }
                    break;
                default:

                    break;
            }

        }

        private void setFrist_Click(object sender, RoutedEventArgs e)
        {
            //list.SelectedItems.Clear();
            //list.SelectedItems.Add(list.Items[2]);
            //list.SelectedItems.Add(list.Items[3]);
            //判断tab页
            switch (tabcontrol1.SelectedIndex)
            {
                case 0:
                    setFristChange(listBox1);
                    break;
                case 1:
                    setFristChange(listBox2);
                    break;
                case 2:
                    setFristChange(listBox3);
                    break;
                case 3:
                    setFristChange(listBox4);
                    break;
                case 4:
                    setFristChange(listBox5);
                    break;
                default:
                    break;
            }
        }

        private void btnLoadPOS_Click(object sender, RoutedEventArgs e)
        {
            //加载POS文件
            System.Windows.Forms.OpenFileDialog pfd = new System.Windows.Forms.OpenFileDialog();
            pfd.Title = "选择POS文件";
            pfd.Multiselect = false;
            if (pfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = pfd.FileName;
                loadPOS loadPOS = new loadPOS();
                POSlist = loadPOS.loadPOSwzTxt(spath);
                if (POSlist == null)
                {
                    tb_status.Text = "读取POS文件失败";
                    return;
                }
                positiveNumb = 0;
                foreach (POSone pp in POSlist)
                {
                    if (pp.State >= 0)
                    {
                        positiveNumb++;
                    }
                }
                tb_status.Text = "导入" + POSlist.Count + "个POS点,其中拍照点" + positiveNumb + "个";


                textBox1.Text = spath;
            }
        }

        private void btnLoadG1_Click(object sender, RoutedEventArgs e)
        {
            //tab1加载照片
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择照片路径";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = fbd.SelectedPath;
                loadImage(listBox1, spath, txtboxG1);
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //没有POS信息不让导入图片
            if (POSlist != null && POSlist.Count > 0)
            {
                btnLoadG1.IsEnabled = true;
                btnLoadG2.IsEnabled = true;
                btnLoadG3.IsEnabled = true;
                btnLoadG4.IsEnabled = true;
                btnLoadG5.IsEnabled = true;
            }
            else
            {
                btnLoadG1.IsEnabled = false;
                btnLoadG2.IsEnabled = false;
                btnLoadG3.IsEnabled = false;
                btnLoadG4.IsEnabled = false;
                btnLoadG5.IsEnabled = false;
            }
        }

        void setFristChange(ListBox _listbox)
        {
            //根据POS的可用点个数筛选选择的照片
            int listviewIndex = _listbox.SelectedIndex;//获得点击的图片index
            int fristIndex = 0;//第一个不为-1的POS点序号
            _listbox.SelectedItem = null;//先全虚

            for (int u = 0; u < POSlist.Count; u++)//找到第一个不为-1的POS点
            {
                if (POSlist[u].State > 0)
                {
                    fristIndex = u;
                    break;
                }
            }
            for (int i = fristIndex; i < POSlist.Count; i++)//根据POS信息改变颜色
            {
                if (listviewIndex + i - fristIndex >= _listbox.Items.Count)
                {
                    tb_status.Text = "POS点数量大于照片数量，请检查照片文件夹和POS信息";
                    return;
                }

                if (POSlist[i].State > 0)
                {
                    //listview.Items[listviewIndex + i - fristIndex].BackColor = Color.FromArgb(148, 229, 77);
                    //listview.Items[listviewIndex + i - fristIndex].ForeColor = Color.FromArgb(77, 154, 0);
                    //listview.Items[listviewIndex + i - fristIndex].Font = new Font(listview.Items[listviewIndex + i - fristIndex].Font, FontStyle.Bold);
                    _listbox.SelectedItems.Add(_listbox.Items[listviewIndex + i - fristIndex]);
                }
                else if (POSlist[i].State < 0)
                {
                    //listview.Items[listviewIndex + i - fristIndex].ForeColor = Color.FromArgb(242, 242, 242);
                    ////listview.Items[listviewIndex + i - fristIndex].BackColor = Color.Empty;
                    //listview.Items[listviewIndex + i - fristIndex].Font = new Font(listview.Items[listviewIndex + i - fristIndex].Font, FontStyle.Regular);
                }
            }
        }

        private void btnSetFrist_Click(object sender, EventArgs e)
        {
            switch (tabcontrol1.SelectedIndex)
            {
                case 0:
                    setFristChange(listBox1);
                    break;
                case 1:
                    setFristChange(listBox2);
                    break;
                case 2:
                    setFristChange(listBox3);
                    break;
                case 3:
                    setFristChange(listBox4);
                    break;
                case 4:
                    setFristChange(listBox5);
                    break;
                default:
                    break;
            }
        }

        private void btnLoadG2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择照片路径";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = fbd.SelectedPath;
                loadImage(listBox2, spath, txtboxG2);
            }
        }

        private void btnLoadG3_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择照片路径";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = fbd.SelectedPath;
                loadImage(listBox3, spath, txtboxG3);
            }
        }

        private void btnLoadG4_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择照片路径";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = fbd.SelectedPath;
                loadImage(listBox4, spath, txtboxG4);
            }
        }

        private void btnLoadG5_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择照片路径";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string spath = fbd.SelectedPath;
                loadImage(listBox5, spath, txtboxG5);
            }
        }


        //保存xml路径
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Title = "保存block文件";
            saveFileDialog1.FileName = "Export";
            saveFileDialog1.Filter = "(*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtboxSaveFilePath.Text = saveFileDialog1.FileName;
            }
        }
        //转存照片路径
        private void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtboxSaveImagePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        //保存使用参数
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.picLong = textBox2.Text;
                Properties.Settings.Default.picWidth = textBox3.Text;
                Properties.Settings.Default.sensorLong = textBox4.Text;
                Properties.Settings.Default.focalLength = textBox5.Text;
                double sl;
                int pl;
                sl = double.Parse(Properties.Settings.Default.sensorLong);
                pl = int.Parse(Properties.Settings.Default.picLong);
                Properties.Settings.Default.pixSize = (sl / pl).ToString();

                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {

            }
            
        }

        string allImagePath;
        int picHeight, picWidth;
        double pixSize, focalLength;
        //生成xml文件按钮
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //读参数
            allImagePath = txtboxSaveImagePath.Text;
            picHeight = int.Parse(textBox3.Text);//
            picWidth = int.Parse(textBox2.Text);
            focalLength = double.Parse(textBox5.Text);
            pixSize = double.Parse(textBox4.Text);

            //POS信息没有的话不生成
            if (POSlist.Count == 0)
            {
                tb_status.Text = "pos数据为空";
                return;
            }
            if (listBox1.Items.Count == 0 &&
               listBox2.Items.Count == 0 &&
               listBox3.Items.Count == 0 &&
               listBox4.Items.Count == 0 &&
               listBox5.Items.Count == 0
               )
            {
                tb_status.Text = "照片数据为空";
                return;
            }
            if (txtboxSaveFilePath.Text == "")
            {
                tb_status.Text = "生成路径为空";
                return;
            }
            //定义坐标系
            string srs = "EPSG:4326";
            switch (comboBox1.SelectedItem)
            {
                case "WGS84":
                    srs = "EPSG:4326";
                    break;
                default:
                    break;
            }
            //定义角度
            bool usingRadins = false;
            if (radioButton1.IsChecked == true)
            {
                usingRadins = false;
            }
            else if (radioButton2.IsChecked == true)
            {
                usingRadins = true;
            }
            //组合POSlist
            makeupPosList();

            //起线程移动照片
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
            {
                moveAllImage();
            }));

            //生成xml tree
            createTree(srs);
            tb_status.Text = "xml导出完成";
        }

        private void makexml(string filePath, int picLong, int sensorLong, int focalLength, string srs, bool usingRadins, string imagePath)
        {
            idNumber = 0;
            idGroupNumber = 0;
            createTree(srs);
        }

        //建主结构
        void createTree(string srs)
        {
            XElement BlocksExchange = new XElement("BlocksExchange",
                new XAttribute("version", "2.1"),
                    new XElement("SpatialReferenceSystems",
                        new XElement("SRS",
                            new XElement("Id", "0"),
                            new XElement("Name", comboBox1.SelectionBoxItem),
                            new XElement("Definition", srs)
                        )
                    ),
                    new XElement("BaseImagePath", txtboxSaveImagePath.Text),
                    new XElement("Block",
                        new XElement("Name", "autoGenerated"),
                        new XElement("Description", "autoGenerated dataset"),
                        new XElement("Type", "Aerial"),
                        new XElement("SRSId", "0"),
                        new XElement("Photogroups", null),

                        new XElement("ControlPoints",null)
                        )

                    );

            if (txtboxG1.Text != "") insertPhotogroup(ref BlocksExchange, 0); //插入照片组
            if (txtboxG2.Text != "") insertPhotogroup(ref BlocksExchange, 1);
            if (txtboxG3.Text != "") insertPhotogroup(ref BlocksExchange, 2);
            if (txtboxG4.Text != "") insertPhotogroup(ref BlocksExchange, 3);
            if (txtboxG5.Text != "") insertPhotogroup(ref BlocksExchange, 4);

            BlocksExchange.Save(txtboxSaveFilePath.Text);

        }

        int idNumber = 0;
        int idGroupNumber = 0;
        void insertPhotogroup(ref XElement root, int numth)
        {
            //查找照片组节点
            //idNumber = 0;block中的照片id不能重复
            IEnumerable<XElement> photogroups =
                from el in root.Descendants()
                where el.Name == "Photogroups"
                select el;

            foreach (XElement el in photogroups)
            {
                XElement photogroup = new XElement("Photogroup",
                                new XElement("Name", "Group"+idGroupNumber++.ToString()),
                                new XElement("ImageDimensions",
                                    new XElement("Width", picWidth),
                                    new XElement("Height", picHeight)
                                ),
                                new XElement("CameraModelType", "Perspective"),
                                new XElement("SensorSize", pixSize.ToString()),
                                new XElement("FocalLength", focalLength.ToString())
                        );

                foreach (POSone item in POSlist)
                {

                    string photopath;
                    switch (numth)
                    {
                        case 0:
                            photopath = item.Imagezs;
                            break;
                        case 1:
                            photopath = item.Image1;
                            break;
                        case 2:
                            photopath = item.Image2;
                            break;
                        case 3:
                            photopath = item.Image3;
                            break;
                        case 4:
                            photopath = item.Image4;
                            break;
                        default:
                            photopath = "";
                            break;
                    }


                    if (item.State > 0)
                    {
                        photogroup.Add(new XElement("Photo",
                            new XElement("Id", idNumber++),
                            new XElement("ImagePath", photopath.ToString()),
                            new XElement("Pose",
                                //new XElement("Rotation",
                                //    new XElement("Heading", item.Heading),
                                //    new XElement("Pitch", item.Pitch),
                                //    new XElement("Roll", item.Roll)
                                //    ),
                                new XElement("Center",
                                    new XElement("x",item.Longitude),
                                    new XElement("y",item.Latitude),
                                    new XElement("z",item.Height)
                                ))));
                    }
                }
                el.Add(photogroup);
            }
        }

        void makeupPosList()
        {
            if (listBox1.Items.Count != 0) insert2PosList(listBox1, 0);
            if (listBox2.Items.Count != 0) insert2PosList(listBox2, 1);
            if (listBox3.Items.Count != 0) insert2PosList(listBox3, 2);
            if (listBox4.Items.Count != 0) insert2PosList(listBox4, 3);
            if (listBox5.Items.Count != 0) insert2PosList(listBox5, 4);
        }
        //整理POSlist
        void insert2PosList(ListBox _listBox, int index)
        {
            if (_listBox.Items.Count != 0)
            {
                positiveNumb = 0;
                for (int i = 0; i < POSlist.Count - 1; i++)
                {
                    if (POSlist[i].State < 0) continue;
                    Image ii = _listBox.SelectedItems[positiveNumb] as Image;
                    string[] s1 = ii.Source.ToString().Split('/');
                    switch (index)
                    {

                        case 0:
                            POSlist[i].Imagezs = "A" + s1[s1.Length - 1]; //涉及到移动，改名
                            positiveNumb++;
                            break;
                        case 1:
                            POSlist[i].Image1 = "B" + s1[s1.Length - 1];
                            positiveNumb++;
                            break;
                        case 2:
                            POSlist[i].Image2 = "C" + s1[s1.Length - 1];
                            positiveNumb++;
                            break;
                        case 3:
                            POSlist[i].Image3 = "D" + s1[s1.Length - 1];
                            positiveNumb++;
                            break;
                        case 4:
                            POSlist[i].Image4 = "E" + s1[s1.Length - 1];
                            positiveNumb++;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                tb_status.Text = "没有导入图片";
                return;
            }
        }

        void moveAllImage()
        {
            string targetPath = allImagePath;
            moveImage(targetPath);
        }
        void moveImage(string targetPath)
        {
            tb_status.Text = "正在移动照片……";
            foreach (POSone item in POSlist)
            {
                if (item.State > 0)
                {
                    if (item.Imagezs != null)
                    {
                        string oPath = txtboxG1.Text + "\\" + item.Imagezs.Substring(1);
                        File.Copy(oPath, targetPath + "\\" + item.Imagezs);
                    }
                    if (item.Image1 != null)
                    {
                        string oPath = txtboxG2.Text + "\\" + item.Image1.Substring(1);
                        File.Copy(oPath, targetPath + "\\" + item.Image1);
                    }
                    if (item.Image2 != null)
                    {
                        string oPath = txtboxG3.Text + "\\" + item.Image2.Substring(1);
                        File.Copy(oPath, targetPath + "\\" + item.Image2);
                    }
                    if (item.Image3 != null)
                    {
                        string oPath = txtboxG4.Text + "\\" + item.Image3.Substring(1);
                        File.Copy(oPath, targetPath + "\\" + item.Image3);
                    }
                    if (item.Image4 != null)
                    {
                        string oPath = txtboxG5.Text + "\\" + item.Image4.Substring(1);
                        File.Copy(oPath, targetPath + "\\" + item.Image4);
                    }
                }
            }
            tb_status.Text = "完成照片移动";
        }
    }
}
