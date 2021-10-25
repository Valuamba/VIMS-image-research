using System;
using System.Windows.Media;

namespace ImageResearchNew.Model.ViewOptions
{
    public class GridOptions
    {
        private readonly int width;
        private readonly int height;

        public GridOptions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        private int _gridSize = 8;
        public int GridSize 
        {
            get => _gridSize;
            set
            {
                _gridSize = value;
                ChangeGridOprtionProperty?.Invoke();
            }
        }

        private bool _isHidden = true;
        public bool IsHidden 
        { 
            get => _isHidden; 
            set
            {
                _isHidden = value;
                ChangeGridOprtionProperty?.Invoke();
            }
        }

        public DrawLayer GridDrawLayer => new DrawLayer((dc) =>
        {
            var pen = new Pen(Brushes.White, 1);

            for (int i = 0; i < width; i += GridSize)
            {
                dc.DrawLine(pen, new System.Windows.Point(i, 0), new System.Windows.Point(i, height));
            }

            for (int j = 0; j < height; j += GridSize)
            {
                dc.DrawLine(pen, new System.Windows.Point(0, j), new System.Windows.Point(width, j));
            }
        }, isHidden: IsHidden);

        public event Action ChangeGridOprtionProperty;
    }
}
