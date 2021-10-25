using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using WindowsPoint = System.Windows.Point;

namespace ImageResearchNew.Model.ImagePlaceTools
{
    public class RegionsTool : ImagePlaceTool
    {
        private WindowsPoint? _start = null;
        private List<Point> highlitedEndPoints = new List<Point>();
        private List<Point> highlitedStartPoints = new List<Point>();

        public RegionsTool(ImageProcessorInfo imageInfo) : base(imageInfo)
        {
        }

        private void Clear()
        {
            imageInfo.DrawLayers.Clear();
            highlitedEndPoints.Clear();
            if (imageInfo != null)
            {
                imageInfo.SelectedBitmap?.Dispose();
                imageInfo.SelectedBitmap = null;
            }
        }

        public override void OnMouseDown(WindowsPoint position)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Clear();
            }

            _start = position;
        }

        public override void OnMouseMove(WindowsPoint position)
        {
            if (_start.HasValue)
            {
                UpdateFactory(position, true);
            }
        }

        public override void OnMouseUp(WindowsPoint position)
        {
            if (_start.HasValue)
            {
                UpdateFactory(position);
            }
            SelectBitmap();
            _start = null;
        }

        private void SelectBitmap()
        {
            if (highlitedStartPoints.Count > 0 && highlitedEndPoints.Count > 0)
            {
                var startPoint = highlitedStartPoints.First();
                var endPoint = highlitedEndPoints.Last();

                if (endPoint.X > startPoint.X && endPoint.Y > startPoint.Y)
                {
                    imageInfo.SelectedBitmap = imageInfo.ViewingImage.Clone(
                        new System.Drawing.Rectangle((int)startPoint.X, (int)startPoint.Y, (int)(endPoint.X - startPoint.X), (int)(endPoint.Y - startPoint.Y)),
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }
            }
        }

        private void UpdateFactory(WindowsPoint position, bool shouldClear = false)
        {
            var brush = new SolidColorBrush(Colors.Blue) { Opacity = 0.3 };
            var region = GetRegion(_start, position);
            if (!region.IsEmpty)
            {
                if (shouldClear)
                {
                    imageInfo.DrawLayers.Clear();
                }
                imageInfo.DrawLayers.Add(new DrawLayer((dc) => dc.DrawRectangle(brush, new Pen(), region)));
            }
        }

        private Rect GetRegion(WindowsPoint? startPosition, WindowsPoint position)
        {
            var gridSize = imageInfo.GridOptions.GridSize;
            var startX = (int)(startPosition.Value.X / gridSize) * gridSize;
            var startY = (int)(startPosition.Value.Y / gridSize) * gridSize;
            var endX = (int)(position.X / gridSize) * gridSize + gridSize;
            var endY = (int)(position.Y / gridSize) * gridSize + gridSize;

            if (endX > imageInfo.ViewingImage.Width)
                endX = imageInfo.ViewingImage.Width;
            if (endY > imageInfo.ViewingImage.Height)
                endY = imageInfo.ViewingImage.Height;

            if (!highlitedEndPoints.Contains(new WindowsPoint(endX, endY)))
            {
                if(!highlitedStartPoints.Contains(new WindowsPoint(startX, startY)))
                {
                    highlitedStartPoints.Add(new WindowsPoint(startX, startY));
                }
                highlitedEndPoints.Add(new WindowsPoint(endX, endY));
                return new Rect(startX, startY, Math.Max(endX - startX, gridSize), Math.Max(endY - startY, gridSize));
            }
            return Rect.Empty;
        }
    }
}
