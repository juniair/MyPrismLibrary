using ImageProcessor;
using ImageProcessor.Imaging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.PSD;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TTImageProsessingLibrary.Model;

namespace TTImageProsessingLibrary
{
    public class TTImageProsessing : BindableBase, ITTImageProsessing
    {
        #region AnimatedGIFSpit Member
        private const int UNKNOWN_LOOP_COUNT = -100;
        private const int PROPERTY_TAG_FRAME_DELAY = 0x5100;
        // For an animated GIF image, the number of times to display the animation. 
        // A value of 0 specifies that the animation should be displayed infinitely
        private const int PROPERTY_TAG_FRAME_COUNT = 0x5101;

        private int FrameCount;
        private bool Animated;
        private int[] FrameDelay;
        private int LoopCount;

        private IList<FrameFile> ImageFrameList;
        private IList<byte[]> FrameStreamDataList;

        private Dictionary<Guid, ImageFormat> GuidToImageFormatMap;

        private FolderBrowserDialog FolderDialog;

        private Microsoft.Win32.OpenFileDialog FileDialog;

        private Image Image;
        private ImageFormat ImageFormat;

        private BitmapEncoder Encoder;

        private ImageFile imageFile;
        public ImageFile ImageFile
        {
            get { return imageFile; }
            set { SetProperty(ref imageFile, value); }
        }
        #endregion


        public TTImageProsessing()
        {
            GuidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
            {
                { ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                { ImageFormat.Gif.Guid,  ImageFormat.Png},
                { ImageFormat.Icon.Guid, ImageFormat.Png},
                { ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                { ImageFormat.Png.Guid,  ImageFormat.Png}
            };


            ImageFrameList = new List<FrameFile>();
            FrameStreamDataList = new List<byte[]>();

            FolderDialog = new FolderBrowserDialog();

            FileDialog = new Microsoft.Win32.OpenFileDialog();
            FileDialog.DefaultExt = "jpg";
            FileDialog.Filter = "이미지(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.psd";

            SaveDialog = new SaveFileDialog();
            SaveDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";
            SaveDialog.Title = "Save an Image File";

            Encoder = new PngBitmapEncoder();

            LoopCount = UNKNOWN_LOOP_COUNT;
            LoopState = "Loop State : ";
            FrameName = "Frmae Name : ";
            FrameDuration = "Frame Duration : ";

            PsdFile = new PsdFile();
            regex = new Regex(@"(\.jpg|jpeg|gif|bmp|png|psd)", RegexOptions.IgnoreCase);
        }

        #region MainImageProsessing
        private byte[] input;
        public byte[] Input
        {
            get { return input; }
            set { SetProperty(ref input, value); }
        }

        private MemoryStream Process;


        private Rectangle rec;
        public Rectangle Rec
        {
            get { return rec; }
            set { SetProperty(ref rec, value); }
        }

        private int x;
        public int X
        {
            get { return x; }
            set { SetProperty(ref x, value); }
        }

        private int y;
        public int Y
        {
            get { return y; }
            set { SetProperty(ref y, value); }
        }

        private int w;
        public int W
        {
            get { return w; }
            set { SetProperty(ref w, value); }
        }

        private int h;
        public int H
        {
            get { return h; }
            set { SetProperty(ref h, value); }
        }

        private SaveFileDialog SaveDialog;

        public void LoadImage()
        {
            Input = OpenImage();
            Bitmap img = new Bitmap(BytearrToImage(Input));
            Rec = new Rectangle(0, 0, img.Width, img.Height);
            X = Rec.X;
            Y = Rec.Y;
            W = Rec.Width;
            H = Rec.Height;

        }

        public void SaveImage()
        {

            using (var imageFactory = new ImageFactory(preserveExifData: true))
            {
                SaveDialog.ShowDialog();

                if (SaveDialog.FileName != "")
                {
                    string savePath = SaveDialog.FileName;
                    imageFactory.Load(Input)
                    .Save(savePath);
                }
            }

        }

        public void ResizeImage(object obj)
        {
            string str = (obj is string) ? obj as string : string.Empty;

            ImageProcessor.Imaging.ResizeMode mode;

            if (str == "max")
                mode = ImageProcessor.Imaging.ResizeMode.Max;
            else
                mode = ImageProcessor.Imaging.ResizeMode.Stretch;

            using (Process = new MemoryStream())
            {
                var size = new System.Drawing.Size(W, H);
                Rec = new Rectangle(Rec.X, Rec.Y, size.Width, size.Height);
                ResizeLayer resizeLayer = new ResizeLayer(size, mode, AnchorPosition.Center, true, null, null, null, null);

                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(Input)
                        .Resize(resizeLayer)
                        .Save(Process);
                }

                SaveProcess();

            }
        }

        public void CropImage()
        {
            using (Process = new MemoryStream())
            {

                Rec = new Rectangle(X, Y, W, H);
                var crop = new ImageProcessor.Imaging.CropLayer(Rec.X, Rec.Y, Rec.Width, Rec.Height, CropMode.Pixels);

                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(Input)
                        .Crop(crop)
                        .Save(Process);
                }

                SaveProcess();

            }

        }

        public void CropTransparentImage()
        {
            using (Process = new MemoryStream())
            {
                Rec = SaveRecInfo(new Bitmap(BytearrToImage(Input)));
                var crop = new ImageProcessor.Imaging.CropLayer(Rec.X, Rec.Y, Rec.Width, Rec.Height, CropMode.Pixels);

                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(Input)
                        .Crop(crop)
                        .Save(Process);
                }

                SaveProcess();

            }
        }

        public void MergeImage()
        {
            Bitmap img = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(img);

            Image source1 = BytearrToImage(Input);
            Image source2 = BytearrToImage(OpenImage());

            Rectangle Rec2 = new Rectangle(200, 300, source2.Width, source2.Height);

            g.DrawImage(source1, Rec);
            g.DrawImage(source2, Rec2);


            using (Process = new MemoryStream())
            {
                img.Save(Process, System.Drawing.Imaging.ImageFormat.Png);
                img = new Bitmap(Process);

                Rec = SaveRecInfo(img);
                var crop = new ImageProcessor.Imaging.CropLayer(Rec.X, Rec.Y, Rec.Width, Rec.Height, CropMode.Pixels);

                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(img)
                        .Crop(crop)
                        .Save(Process);
                }

                SaveProcess();
            }
        }

        public void SplitGif()
        {
            LoadImage();

            AnimatedImageSplit();
            GetAnimatedImageLoopCount();
            ConfigurationFrameSaveDirectoryPath();
            SaveFrame();

            ImageFile = new ImageFile
            {
                FrameList = new System.Collections.ObjectModel.ObservableCollection<FrameFile>(ImageFrameList),
                LoopCount = this.LoopCount,
            };
        }




        // byte[] -> image
        private Image BytearrToImage(byte[] bytearr)
        {
            MemoryStream ms = new MemoryStream(bytearr);
            Image img = Image.FromStream(ms);
            return img;
        }

        //이미지 가져오기( + .psd)
        private byte[] OpenImage()
        {
            FileDialog.ShowDialog();
            
            if (FileDialog.FileNames.Length > 0)
            {
                string[] tokens = regex.Split(FileDialog.SafeFileName);

                foreach (string fileName in FileDialog.FileNames)
                {
                    if (tokens[1].ToLower().Equals("psd"))
                    {
                        
                        PsdFile.Load(fileName);
                        
                        using (Process = new MemoryStream())
                        {
                            using (var imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                Bitmap bitmap = ImageDecoder.DecodeImage(PsdFile);
                                imageFactory.Load(bitmap)
                                    .Save(Process);
                            }
                            return Process.ToArray();
                        }
                    }
                    else
                    {
                        return File.ReadAllBytes(fileName);
                    }
                }
            }
            return null;
        }

        private void SaveProcess()
        {
            Input = Process.ToArray();
            X = Rec.X;
            Y = Rec.Y;
            W = Rec.Width;
            H = Rec.Height;
        }

        private Rectangle SaveRecInfo(Bitmap img)
        {
            return new Rectangle(MinX(img), MinY(img), MaxX(img) - MinX(img), MaxY(img) - MinY(img));
        }

        private int MinX(Bitmap img)
        {
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    System.Drawing.Color col = img.GetPixel(x, y);
                    if (col.A != 0)
                    {
                        return x;
                    }
                }
            }
            return 0;
        }

        private int MinY(Bitmap img)
        {
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    System.Drawing.Color col = img.GetPixel(x, y);
                    if (col.A != 0)
                    {
                        return y;
                    }
                }
            }
            return 0;
        }

        private int MaxX(Bitmap img)
        {
            for (int x = img.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    System.Drawing.Color col = img.GetPixel(x, y);
                    if (col.A != 0)
                    {
                        return x;
                    }
                }
            }
            return img.Width;
        }

        private int MaxY(Bitmap img)
        {
            for (int y = img.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    System.Drawing.Color col = img.GetPixel(x, y);
                    if (col.A != 0)
                    {
                        return y;
                    }
                }
            }
            return img.Height;
        }
        #endregion

        #region SplitGIF 에서 처리 되는 메소드
        private void AnimatedImageSplit()
        {
            if (!FileDialog.FileName.Equals(""))
            {
                if (ImageFrameList.Count != 0)
                {
                    ImageFrameList.Clear();
                }

                if (FrameStreamDataList.Count != 0)
                {
                    FrameStreamDataList.Clear();
                }

                Image = Image.FromFile(FileDialog.FileName);
                Guid imageGuid = Image.RawFormat.Guid;

                ImageFormat = null;

                foreach (KeyValuePair<Guid, ImageFormat> pair in GuidToImageFormatMap)
                {
                    if (imageGuid == pair.Key)
                    {
                        ImageFormat = pair.Value;
                        break;
                    }
                }

                if (ImageFormat == null)
                {
                    throw new FileFormatException("해당 파일에 맞는 확장자가 존재하지 않습니다.");
                }


                Animated = ImageAnimator.CanAnimate(Image);

                if (Animated)
                {
                    FrameCount = Image.GetFrameCount(FrameDimension.Time);
                    PropertyItem frameDelayItem = Image.GetPropertyItem(PROPERTY_TAG_FRAME_DELAY);


                    if (frameDelayItem != null)
                    {
                        byte[] values = frameDelayItem.Value;
                        FrameDelay = new int[FrameCount];

                        for (int i = 0; i < FrameCount; ++i)
                        {
                            FrameDelay[i] = values[i * 4] + 256 * values[i * 4 + 1] + 256 * 256 * values[i * 4 + 2] + 256 * 256 * 256 * values[i * 4 + 3] * 10;

                            Image.SelectActiveFrame(FrameDimension.Time, i);
                            using (MemoryStream memortStream = new MemoryStream())
                            {
                                Image.Save(memortStream, ImageFormat);
                                FrameStreamDataList.Add(memortStream.ToArray());
                            }
                        }
                    }
                    else
                    {
                        FrameCount = 1;
                        Image.SelectActiveFrame(FrameDimension.Time, 0);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image.Save(ms, ImageFormat);
                            FrameStreamDataList.Add(ms.ToArray());
                        }
                    }

                }

                if (FrameDelay == null)
                {
                    FrameDelay = new int[FrameCount];
                }
            }
        }
        private void GetAnimatedImageLoopCount()
        {
            if (!FileDialog.FileName.Equals(""))
            {
                if (Animated)
                {
                    PropertyItem frameLoopItem = Image.GetPropertyItem(PROPERTY_TAG_FRAME_COUNT);
                    if (frameLoopItem != null)
                    {
                        LoopCount = BitConverter.ToInt16(frameLoopItem.Value, 0);
                        if (frameLoopItem.Value[0] != 0)
                        {
                            LoopCount++;
                        }
                    }
                }
                else
                {
                    LoopCount = -1;
                }
            }

        }


        private void ConfigurationFrameSaveDirectoryPath()
        {
            if (Animated)
            {
                FolderDialog.ShowDialog();
            }
        }

        private void SaveFrame()
        {
            if (!FolderDialog.SelectedPath.Equals(""))
            {
                if (Animated)
                {
                    string framePath;
                    string frameName;
                    for (int i = 0; i < FrameCount; i++)
                    {
                        
                        frameName = string.Format(@"{0}_frame[{1}].{2}", regex.Split(FileDialog.SafeFileName)[0], i, ImageFormat.ToString());
                        framePath = string.Format(@"{0}\\{1}", FolderDialog.SelectedPath, frameName);
                        using (Image image = Image.FromStream(new MemoryStream(FrameStreamDataList[i])))
                        {
                            image.Save(framePath, ImageFormat);
                        }

                        ImageFrameList.Add(new FrameFile
                        {
                            Name = frameName,
                            Path = framePath,
                            Source = FrameStreamDataList[i],
                            Duration = FrameDelay[i],
                        });
                    }
                }
                else
                {
                    ImageFrameList.Add(new FrameFile
                    {
                        Name = FileDialog.SafeFileName,
                        Path = FileDialog.FileName,
                        Source = FrameStreamDataList[0],
                        Duration = FrameDelay[0],
                    });
                }
            }
            else
            {
                for (int i = 0; i < FrameCount; i++)
                {
                    ImageFrameList.Add(new FrameFile
                    {
                        Name = string.Format(@"{0}_frame[{1}].{2}", regex.Split(FileDialog.SafeFileName)[0], i, ImageFormat.ToString()),
                        Path = null,
                        Source = FrameStreamDataList[i],
                        Duration = FrameDelay[i],
                    });
                }
            }

            if(LoopCount > 0)
            {
                LoopState = string.Format("LoopState : {0}회 반복", LoopCount);
            }
            else if (LoopCount == 0)
            {
                LoopState = "LoopState : 무한 반복";
            }
            else if(LoopCount == -1)
            {
                LoopState = "LoopState : 일반 이미지";
            }
            else
            {

            }
        }
        #endregion


        private string loopState;
        public string LoopState
        {
            get { return loopState; }
            set { SetProperty(ref loopState, value); }
        }

        private string frameName;
        public string FrameName
        {
            get { return frameName; }
            set { SetProperty(ref frameName, value); }
        }

        private string frameDuration;
        public string FrameDuration
        {
            get { return frameDuration; }
            set { SetProperty(ref frameDuration, value); }
        }

        public void OnItemSelected(object[] selectedItems)
        {
            if (selectedItems != null && selectedItems.Length > 0)
            {
                FrameFile frame = selectedItems.FirstOrDefault() as FrameFile;
                if (frame != null)
                {
                    FrameName = string.Format("FramenName : {0}", frame.Name);
                    FrameDuration = string.Format("FramenDuration : {0} ms", frame.Duration);

                }
            }
        }

        private PsdFile PsdFile;
        private Regex regex;

        #region 2017-04-12 레이아웃 별 추가 메소드
        public void SaveLayer()
        {
            var result = FolderDialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                string[] tokens = regex.Split(FileDialog.SafeFileName);

                if(tokens[1].ToLower().Equals("psd"))
                {
                    using(var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        Bitmap bitmap;
                        foreach (Layer layer in PsdFile.Layers)
                        {
                            bitmap = ImageDecoder.DecodeImage(layer);
                            if(bitmap != null)
                            {
                                imageFactory.Load(bitmap).Save(string.Format((@"{0}\{1}.jpg", FolderDialog.SelectedPath, layer.Name));
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }

}
