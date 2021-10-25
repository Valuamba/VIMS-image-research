using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ImageResearch.Engine.Model;
using ImageResearch.Engine.Processing;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace ImageResearch.Engine
{
    public class ImageFactory
    {
        public Bitmap Bitmap { get => Image.ToBitmap(); }
        public Image<Bgr, Byte> Image;

        public ImageFactory(Bitmap bitmap)
        {
            Image = bitmap.ToImage<Bgr, Byte>();
        }

        public ImageFactory()
        {

        }
        
        public Bitmap Quant(int level)
        {
            var bitmap = Bitmap;
            for (int x = 0; x < Image.Width; x++)
            {
                for (int y = 0; y < Image.Height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int myRed = c.R, myGreen = c.G, myBlue = c.B;

                    int R = ((myRed / (255 / level)) * (255 / level));
                    int G = ((myGreen / (255 / level)) * (255 / level));
                    int B = ((myBlue / (255 / level)) * (255 / level));

                    Color pixelColor = bitmap.GetPixel(x, y);
                    Color newColor = Color.FromArgb(R, G, B);
                    bitmap.SetPixel(x, y, newColor);
                }
            }
            return bitmap;
        }

        private void Rgb2Yuv(Image<Bgr, Byte> img, Image<Gray, double> yImg, Image<Gray, double> uImg, Image<Gray, double> vImg)
        {
            double Y = 0.0, U = 0.0, V = 0.0;
            for (int h = 0; h < img.Height; ++h)
            {
                for (int w = 0; w < img.Width; ++w)
                {
                    Y = 0.257 * img.Data[h, w, 0] + 0.504 * img.Data[h, w, 1] + 0.098 * img.Data[h, w, 2] + 16;
                    U = 0.148 * img.Data[h, w, 0] - 0.291 * img.Data[h, w, 1] + 0.439 * img.Data[h, w, 2] + 128;
                    V = 0.439 * img.Data[h, w, 0] - 0.368 * img.Data[h, w, 1] - 0.071 * img.Data[h, w, 2] + 128;
                    yImg.Data[h, w, 0] = Y;
                    uImg.Data[h, w, 0] = U;
                    vImg.Data[h, w, 0] = V;
                }
            }
        }

        public Vectorscope[] GetVectoroscopeOfImage(Bitmap bitmap)
        {
            var bitData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var vector = new Vectorscope[width * height];
            int counter = 0;
            unsafe
            {
                var ptr = (byte*)bitData.Scan0;

                for (int i = 0; i < height; i++)
                {
                    var ptr2 = ptr;
                    for (int j = 0; j < width; j++, ptr2 += 3)
                    {
                        var Ry = ptr2[0] * 0.7 - 0.59 * ptr2[1] - 0.11 * ptr2[2];
                        var By = ptr2[0] * (-0.3) - 0.59 * ptr2[1] + 0.89 * ptr2[2];
                        vector[counter++] = new Vectorscope((int)Ry, (int)By);
                    }
                    ptr += bitData.Stride;
                }
            }
            bitmap.UnlockBits(bitData);
            return vector;
        }

        public int[] BuildHistogram(Bitmap bitmap)
        {
            return new Histogram().GetHistogram(bitmap);
        }

        public Bitmap ApplyFilter(float[,] matrix)
        {
            ConvolutionKernelF convMatrix = new ConvolutionKernelF(matrix);
            CvInvoke.Filter2D(Image, Image, convMatrix, new Point(-1, -1));
            return Image.ToBitmap();
        }

        private byte ReduceValue(byte b, byte rate)
        {
            return (byte)((b / rate) * rate);
        }

        public Bitmap Quant(Bitmap bmp, int dstLevel, int srcLevel = 8)
        {
            var bitmap = new Bitmap(bmp);
            var bitData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var width = bitmap.Width;
            var height = bitmap.Height;

            var qunatizeRate = (byte)Math.Pow(2, srcLevel - dstLevel);

            unsafe
            {
                var ptr = (byte*)bitData.Scan0;

                for (int i = 0; i < height; i++)
                {
                    var ptr2 = ptr;
                    for (int j = 0; j < width; j++, ptr2 += 3)
                    {
                        ptr2[0] = ReduceValue(ptr2[0], qunatizeRate);
                        ptr2[1] = ReduceValue(ptr2[1], qunatizeRate);
                        ptr2[2] = ReduceValue(ptr2[2], qunatizeRate);
                    }
                    ptr += bitData.Stride;
                }
            }

            bitmap.UnlockBits(bitData);
            return bitmap;
        }

        public Bitmap Subsampling(int n, bool resizeToSource)
        {
            var size = new Size((int)(Image.Width / n), (int)(Image.Height / n));
            Image<Bgr, Byte> dst = new Image<Bgr, byte>(size);
            CvInvoke.Resize(Image, dst, size, interpolation: Inter.Nearest);

            if(resizeToSource)
            {
                Image<Bgr, Byte> resizedToSource = new Image<Bgr, byte>(new Size(Image.Width, Image.Height));

                CvInvoke.Resize(dst, resizedToSource, new Size(Image.Width, Image.Height), interpolation: Inter.Nearest);
                return resizedToSource.ToBitmap();
            }
            
            return dst.ToBitmap();
        }

        public Bitmap Resize(int dstWidth, int dstHeight)
        {
            var size = new Size(dstWidth, dstHeight);
            Image<Bgr, Byte> dst = new Image<Bgr, byte>(size);
            CvInvoke.Resize(Image, dst, size, interpolation: Inter.Cubic);
            return dst.ToBitmap();
        }

        public void ConvertToYuvFormat(string format = "4:4:4")
        {
            CvInvoke.CvtColor(Image, Image, ColorConversion.Bgr2Yuv);
        }

        public Bitmap ConvertTo(ColorConversion code, int a = 4, int b = 4, int j = 4)
        {
            CvInvoke.CvtColor(Image, Image, code);

            return SubsampleChannels(a, b, j);
        }

        public Bitmap SubsampleChannels(int a, int b, int j)
        {
            VectorOfMat vector = new VectorOfMat();
            CvInvoke.Split(Image, vector);
            VectorOfMat mergedChannelsVector = new VectorOfMat();

            Mat firstLuminanceChannel = new Mat();
            Mat secondLuminanceChannel = new Mat();
            
            if(a == 4 && b == 4 && j == 4)
            {
                return Image.ToBitmap();
            }
            else if (a == 4 && b != 0 && j != 0)
            {
                CvInvoke.Resize(vector[1], firstLuminanceChannel, new Size(Image.Width / (a / b), Image.Height), interpolation: Inter.Nearest);
                CvInvoke.Resize(vector[2], secondLuminanceChannel, new Size(Image.Width / (a / j), Image.Height), interpolation: Inter.Nearest);
            }
            else if (a == 4 && b != 0 && j == 0)
            {
                CvInvoke.Resize(vector[1], firstLuminanceChannel, new Size(Image.Width / (a / b), Image.Height / (a / b)), interpolation: Inter.Nearest);
                CvInvoke.Resize(vector[2], secondLuminanceChannel, new Size(Image.Width / (a / b), Image.Height / (a / b)), interpolation: Inter.Nearest);
            }
            else if (a == 4 && b == 0 && j == 0)
            {
                return vector[0].ToBitmap();
            }

            CvInvoke.Resize(firstLuminanceChannel, vector[1], new Size(Image.Width, Image.Height), interpolation: Inter.Nearest);
            CvInvoke.Resize(secondLuminanceChannel, vector[2], new Size(Image.Width, Image.Height), interpolation: Inter.Nearest);

            CvInvoke.Merge(vector, Image);

            return Image.ToBitmap();
        }

        public Bitmap ChannelRotation(List<bool> channelsInfo)
        {
            VectorOfMat vector = new VectorOfMat();
            Image<Rgb, Byte> rgbImage = new Image<Rgb, byte>(new Size(Image.Width, Image.Height));

            CvInvoke.CvtColor(Image, rgbImage, ColorConversion.Bgr2Rgb);


            CvInvoke.Split(rgbImage, vector);
            VectorOfMat mergedChannelsVector = new VectorOfMat();

            for (int i = 0; i < channelsInfo.Count; i++)
            {
                var mat = vector[i];
                mergedChannelsVector.Push(channelsInfo[i] ? mat : Mat.Zeros(mat.Height, mat.Width, mat.Depth, 1));
            }

            CvInvoke.Merge(mergedChannelsVector, rgbImage);
            return rgbImage.ToBitmap();
        }

        public Bitmap ChannelYUVRotation(List<bool> channelsInfo)
        {
            VectorOfMat vector = new VectorOfMat();
            CvInvoke.Split(Image, vector);
            var outputArray = vector.GetOutputArray();
            VectorOfMat mergedChannelsVector = new VectorOfMat();

            var index = channelsInfo.FindIndex(t => t);

            if (index >= 0)
            {
                mergedChannelsVector.Push(vector[index]);
                mergedChannelsVector.Push(vector[index]);
                mergedChannelsVector.Push(vector[index]);

                CvInvoke.Merge(mergedChannelsVector, Image);
            }
            return Image.ToBitmap();
        }

        public Bitmap GetChannel(int index)
        {
            VectorOfMat vector = new VectorOfMat();
            CvInvoke.Split(Image, vector);
            
            return vector[index].ToBitmap();
        }
    }
}
