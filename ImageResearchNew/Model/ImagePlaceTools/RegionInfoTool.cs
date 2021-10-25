using ImageResearchNew.AppWindows;
using ImageResearchNew.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace ImageResearchNew.Model.ImagePlaceTools
{
    public class RegionInfoTool : ImagePlaceTool
    {
        public RegionInfoTool(ImageProcessorInfo imageInfo) : base(imageInfo)
        {
        }

        private bool _clicked;

        public override void OnMouseDown(System.Windows.Point position)
        {
            _clicked = true;
        }

        public override void OnMouseMove(System.Windows.Point position)
        {

        }

        public override void OnMouseUp(System.Windows.Point position)
        {
            if (_clicked)
            {
                var gridSize = imageInfo.GridOptions.GridSize;
                var startX = (int)(position.X / gridSize) * gridSize;
                var startY = (int)(position.Y / gridSize) * gridSize;

                var pixels = GetPixelsRegion(imageInfo.ViewingImage, startX, startY, gridSize);

                var window = new PixelsWindow();

                window.CreateGrid(pixels);
                window.ShowDialog();
            }

            _clicked = false;
        }

        private Pixel[][] GetPixelsRegion(Bitmap image, int x, int y, int blockSize)
        {
            var pixels = new List<List<Pixel>>();

            var imageX = x / blockSize * blockSize;
            var imageY = y / blockSize * blockSize;

            for (int i = 0; i < pixels.Count; i++)
            {
                pixels[i] = new List<Pixel>();
            }

            for (var j = imageY; j < imageY + blockSize - 1; j++)
            {
                //row
                pixels.Add(new List<Pixel>());

                for (var i = imageX; i < imageX + blockSize - 1; i++)
                {
                    if (i < image.Width && j < image.Height)
                    {
                        pixels.Last().Add(new Pixel(image.GetPixel(i, j)));
                    }
                }
            }

            return pixels.Select(a => a.ToArray()).ToArray();
        }
    }
}
