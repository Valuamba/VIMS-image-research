using System;
using System.Windows.Media;

namespace ImageResearchNew.Model
{
    public class DrawLayer
    {
        public Action<DrawingContext> Method { get; }
        public bool IsHidden { get; set; }

        public DrawLayer(Action<DrawingContext> method, bool isHidden = false)
        {
            IsHidden = isHidden;
            Method = method;
        }
    }
}
