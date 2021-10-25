using ImageResearchNew.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace ImageResearchNew.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap Open(FileInfo file)
        {
            try
            {
                // Selecting a Way to load the Image depending on it's Format:
                switch (file.Extension.ToLower())
                {
                    case ".bmp":
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".tif": return (Bitmap)Image.FromFile(file.FullName);
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource BitmapSourceFromBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;

            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        public static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;

            using (var outStream = new MemoryStream())
            {
                var enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        public static void Save(FileInfo file, Bitmap bitmap)
        {
            try
            {
                // Selecting a Way to save the Image depending on it's Format:
                switch (file.Extension.ToLower())
                {
                    case ".bmp": SaveBitmap(file, bitmap, new BmpBitmapEncoder()); break;
                    case ".png": SaveBitmap(file, bitmap, new PngBitmapEncoder()); break;
                    case ".jpg":
                    case ".jpeg": SaveBitmap(file, bitmap, new JpegBitmapEncoder()); break;
                    case ".gif": SaveBitmap(file, bitmap, new GifBitmapEncoder()); break;
                    case ".tif": SaveBitmap(file, bitmap, new TiffBitmapEncoder()); break;
                    default:
                        throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void SaveBitmap(FileInfo file, Bitmap bitmap, BitmapEncoder encoder)
        {
            encoder.Frames.Add(BitmapFrame.Create(BitmapSourceFromBitmap(bitmap)));

            var output = File.Open(file.FullName, FileMode.OpenOrCreate, FileAccess.Write);

            encoder.Save(output);
            output.Close();
        }
    }
}
