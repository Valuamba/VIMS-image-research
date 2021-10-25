using System.Drawing;
using WindowsPoint = System.Windows.Point;

namespace ImageResearchNew.Model.ImagePlaceTools
{
    public abstract class ImagePlaceTool
    {
        protected readonly ImageProcessorInfo imageInfo;
        
        public ImagePlaceTool(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
        }

        public Bitmap SelectedBitmap { get; private set; }

        public abstract void OnMouseDown(WindowsPoint position);

        public abstract void OnMouseMove(WindowsPoint position);

        public abstract void OnMouseUp(WindowsPoint position);
    }
}
