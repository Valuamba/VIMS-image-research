using ImageResearchNew.Model.ColorModels;
using System;
using System.Drawing;

namespace ImageResearchNew.Model
{
    public class Layer
    {
        public Func<Bitmap, Bitmap> ImageProcessorMethod { get; set; }
        public Func<ColorModel> ColorModelMethod { get; set; }
        public bool IsHidden { get; set; }
        public string Definition { get; set; }

        public Layer(Func<Bitmap, Bitmap> method, bool isHidden = false)
        {
            IsHidden = isHidden;
            ImageProcessorMethod = method;
        }

        public Layer(string definition, bool isHidden = false)
        {
            IsHidden = isHidden;
            Definition = definition;
        }

        public Layer(Func<Bitmap, Bitmap> method, string definition, bool isHidden = false) : this(definition, isHidden)
        {
            ImageProcessorMethod = method;
        }
    }
}
