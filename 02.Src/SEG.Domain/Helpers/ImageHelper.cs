using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SEG.Domain.Helpers
{
    static public class ImageHelper
    {
        static public byte[] LoadToBytes(string filename)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
            return ImageToBytes(img);
        }

        static public System.Drawing.Image BytesToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        static public byte[] ImageToBytes(System.Drawing.Image img)
        {
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            return arr;
        }



        static public System.Drawing.Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new JpegBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }



        static public BitmapSource Bitmap2BitmapSource(System.Drawing.Bitmap b)
        {
            using (MemoryStream ms = new MemoryStream(ImageToBytes(b)))
            {
                var decoder = BitmapDecoder.Create(ms,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }

        static public Image SaveImage(SEGRepository segR, BitmapImage BIimg)
        {
            Image i = null;
            using (SEGContext seg = segR.GetContext())
            {
                // Obtain the bytes of the BitmapImage
                System.Drawing.Image img = BitmapImage2Bitmap(BIimg);
                byte[] b = ImageToBytes(img);                

                // Generate thumb. Thumbs are 200 by 200
                System.Drawing.Image thumb = null;
                if (img.Width > img.Height)
                {
                    int iW = 200;
                    int iH = Convert.ToInt32((img.Height * 1.0) / (img.Width * 1.0) * 200.0);
                    thumb = new System.Drawing.Bitmap(iW, iH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(thumb);
                    gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, iW, iH),
                                    new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                                    System.Drawing.GraphicsUnit.Pixel);
                }
                else
                {
                    int iH = 200;
                    int iW = Convert.ToInt32((img.Width * 1.0) / (img.Height * 1.0) * 200.0);
                    thumb = new System.Drawing.Bitmap(iW, iH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(thumb);
                    gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, iW, iH),
                                    new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                                    System.Drawing.GraphicsUnit.Pixel);
                }

                i = new Image();
                i.Data = b;
                i.Filename = "_none_";
                i.Height = img.Height;
                i.Width = img.Width;
                i.ThumbData = ImageToBytes(thumb);
                i.ThumbWidth = thumb.Width;
                i.ThumbHeight = thumb.Height;
                i.Name = "_none_";

                seg.Images.Add(i);
                seg.SaveChanges();
            }
            return i;
        }

        static public Guid SaveImage(SEGRepository segR, string filename)
        {
            Guid g = Guid.Empty;
            using(SEGContext seg = segR.GetContext())
            {
                byte[] b = LoadToBytes(filename);
                System.Drawing.Image img = BytesToImage(b);
                // Generate thumb. Thumbs are 200 by 200
                System.Drawing.Image thumb = null;
                if(img.Width>img.Height)
                {
                    int iW = 200;
                    int iH = Convert.ToInt32((img.Height * 1.0) / (img.Width * 1.0) * 200.0);
                    thumb = new System.Drawing.Bitmap(iW,iH,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(thumb);
                    gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, iW, iH),
                                    new System.Drawing.Rectangle(0, 0, img.Width, img.Height), 
                                    System.Drawing.GraphicsUnit.Pixel);
                }
                else
                {
                    int iH = 200;
                    int iW = Convert.ToInt32((img.Width * 1.0) / (img.Height * 1.0) * 200.0);
                    thumb = new System.Drawing.Bitmap(iW, iH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(thumb);
                    gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, iW, iH),
                                    new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                                    System.Drawing.GraphicsUnit.Pixel);
                }

                Image i = new Image();
                i.Data = b;
                i.Filename = filename;
                i.Height = img.Height;
                i.Width = img.Width;
                i.ThumbData = ImageToBytes(thumb);
                i.ThumbWidth = thumb.Width;
                i.ThumbHeight = thumb.Height;
                i.Name = Path.GetFileNameWithoutExtension(filename);

                seg.Images.Add(i);
                seg.SaveChanges();
                g = i.Id;
            }
            return g;
        }

        static public void FillImage(Image img)
        {
            System.Drawing.Image b = BytesToImage(img.Data);
            using (MemoryStream memory = new MemoryStream())
            {
                b.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;
                img.DataImage = new BitmapImage();
                img.DataImage.BeginInit();
                img.DataImage.StreamSource = memory;
                img.DataImage.CacheOption = BitmapCacheOption.OnLoad;
                img.DataImage.EndInit();
            }

            b = BytesToImage(img.ThumbData);
            using (MemoryStream memory = new MemoryStream())
            {
                b.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;
                img.ThumbDataImage = new BitmapImage();
                img.ThumbDataImage.BeginInit();
                img.ThumbDataImage.StreamSource = memory;
                img.ThumbDataImage.CacheOption = BitmapCacheOption.OnLoad;
                img.ThumbDataImage.EndInit();
            }
        }

        static byte[] CloneBytes(byte[] array)
        {
            byte[] result = new byte[array.Length];
            Buffer.BlockCopy(array, 0, result, 0, array.Length * sizeof(byte));
            return result;
        }

        static public Guid CopyImage(SEGRepository segR, Guid imgId)
        {
            Guid iNewId = Guid.Empty;
            using (SEGContext seg = segR.GetContext())
            {
                var img = seg.Images.FirstOrDefault(x => x.Id == imgId);
                var iNew = new Image();
                iNew.Data = CloneBytes(img.Data);
                iNew.Filename = img.Filename;
                iNew.Height = img.Height;
                iNew.Name = img.Name;
                iNew.ThumbData = CloneBytes(img.ThumbData);
                iNew.ThumbHeight = img.ThumbHeight;
                iNew.ThumbWidth = img.ThumbWidth;
                iNew.Width = img.Width;
                seg.Images.Add(iNew);
                seg.SaveChanges();

                iNewId = iNew.Id;
            }
            return iNewId;
        }

        static public Image GetImage(SEGRepository segR, Guid imgId)
        {
            Image img = null;
            if (imgId != Guid.Empty)
            {
                using (SEGContext seg = segR.GetContext())
                {
                    img = seg.Images.FirstOrDefault(x => x.Id == imgId);
                }
                if (img != null)
                {
                    System.Drawing.Image b = BytesToImage(img.Data);
                    using (MemoryStream memory = new MemoryStream())
                    {
                        b.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        memory.Position = 0;
                        img.DataImage = new BitmapImage();
                        img.DataImage.BeginInit();
                        img.DataImage.StreamSource = memory;
                        img.DataImage.CacheOption = BitmapCacheOption.OnLoad;
                        img.DataImage.EndInit();
                    }

                    b = BytesToImage(img.ThumbData);
                    using (MemoryStream memory = new MemoryStream())
                    {
                        b.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        memory.Position = 0;
                        img.ThumbDataImage = new BitmapImage();
                        img.ThumbDataImage.BeginInit();
                        img.ThumbDataImage.StreamSource = memory;
                        img.ThumbDataImage.CacheOption = BitmapCacheOption.OnLoad;
                        img.ThumbDataImage.EndInit();
                    }
                }
            }
            return img;
        }

    }
}
