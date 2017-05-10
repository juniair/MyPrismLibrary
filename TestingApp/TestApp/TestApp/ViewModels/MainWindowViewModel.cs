using ImageProcessor;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.PSD;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TestApp.ViewModels
{

    public class Model
    {
        public string Name { get; set; }
    }


    public class MainWindowViewModel : BindableBase
    {
        private string title = "Test App";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand ChangeColorCommand { get; set; }

        private System.Windows.Media.Brush color;
        public System.Windows.Media.Brush LabelColor
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }



        private ObservableCollection<TabItem> tabs;
        public ObservableCollection<TabItem> Tabs
        {
            get { return tabs; }
            set { SetProperty(ref tabs, value); }
        }

        public ICommand SaveCommand { get; private set; }

        public MainWindowViewModel()
        {
            string s = System.IO.Path.GetFileNameWithoutExtension("Asd");
            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = s, Content = "One's content" });
            Tabs.Add(new TabItem { Header = "Two", Content = "Two's content" });
        }

        private void run()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = "jpg";
            fileDialog.Filter = "이미지(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.psd)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.psd";
            fileDialog.Multiselect = true;
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            Debug.WriteLine("test");
            Nullable<bool> result = fileDialog.ShowDialog();
            var folderesult = folderBrowserDialog.ShowDialog();
            if (result == true && folderesult == System.Windows.Forms.DialogResult.OK)
            {

                string[] tokens = Regex.Split(fileDialog.SafeFileName, @"(\.jpg|jpeg|gif|bmp|png|psd)", RegexOptions.IgnoreCase);
                
                if (tokens[1].ToLower().Equals("psd"))
                {

                    PsdFile psd = new PsdFile().Load(fileDialog.FileName);
                    
                    using (var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        Bitmap bitmap;
                        
                        foreach (Layer layer in psd.Layers)
                        {
                            bitmap = ImageDecoder.DecodeImage(layer);
                            if(bitmap != null)
                            {

                                imageFactory.Load(bitmap).Save(string.Format(@"{0}\{1}.jpg", folderBrowserDialog.SelectedPath, layer.Name));
                                //Encoding eucKr = Encoding.GetEncoding("euc-kr");
                                //EncodingInfo[] encods = Encoding.GetEncodings();
                                //Encoding destEnc = Encoding.UTF8;
                                //using (StreamWriter file = new StreamWriter(string.Format(@"{0}\info.txt", folderBrowserDialog.SelectedPath)))
                                //{
                                //    foreach (EncodingInfo ec in encods)
                                //    {
                                //        Encoding enc = ec.GetEncoding();
                                //        byte[] sourceBytes = enc.GetBytes(layer.Name);
                                //        byte[] endBytes = Encoding.Convert(eucKr, destEnc, sourceBytes);
                                //        string info = string.Format("{0}({1}) : {2}", enc.EncodingName, enc.BodyName, destEnc.GetString(endBytes));
                                //        file.WriteLine(info);
                                //    }
                                //}

                                //byte[] byteString1 = Encoding.Default.GetBytes("테스트");
                                //byte[] byteString3 = Encoding.Default.GetBytes("layer 1");
                                //byte[] byteString2 = Encoding.Default.GetBytes(layer.Name);
                                //string layerName1 = Encoding.UTF8.GetString(byteString2);
                                //string layerName2 = Encoding.Default.GetString(byteString2);
                                //imageFactory.Load(bitmap).Save(string.Format(@"{0}\{1}.jpg", folderBrowserDialog.SelectedPath, layerName));
                            }
                            
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("No");
                }

            }
        }

        private void saveLayer(ImageFactory factory, Layer layer, int layerCount, string fileName, string folderPath)
        {
            
            if (layer.MaskData.Layer != null)
            {
                saveLayer(factory, layer.MaskData.Layer, layerCount + 1, fileName, folderPath);
            }
            else
            {
                factory.Load(layer.MaskData.ImageData).Save(string.Format(@"{0}\{1}_{2}.jpg", folderPath, fileName, layerCount));
            }
        }
    }
}
