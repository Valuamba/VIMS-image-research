using ImageResearch.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vims.ImageProcessing.Tests
{
    [TestClass]
    public class BitmapTest
    {
        private const byte R_ChannelValue = 128;
        private const byte G_ChannelValue = 255;
        private const byte B_ChannelValue = 45;

        private const int Width = 25;
        private const int Height = 25;

        public Bitmap InitializeBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            var bitData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), 
                System.Drawing.Imaging.ImageLockMode.ReadWrite, 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                var ptr = (byte*)bitData.Scan0;

                for (int i = 0; i < Height; i++)
                {
                    var ptr2 = ptr;
                    for (int j = 0; j < Width; j++, ptr +=3)
                    {
                        ptr[0] = R_ChannelValue;
                        ptr[1] = G_ChannelValue;
                        ptr[2] = B_ChannelValue;
                    }
                    ptr += bitData.Stride;
                }
            }
            bitmap.UnlockBits(bitData);

            return bitmap;
        }

        [TestMethod]
        public void TestSeparatedChannels()
        {
            var bitmap = InitializeBitmap();
            var blueChannel = new ImageFactory(bitmap).GetChannel(0);
            for(int i = 0; i < bitmap.Height; i++)
            {
                for(int j = 0; j < bitmap.Width; j++)
                {
                    var pixel = blueChannel.GetPixel(j, i);

                }
            }
        }
    }
}
