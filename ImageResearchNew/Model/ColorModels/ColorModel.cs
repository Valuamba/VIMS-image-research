using System.Collections.Generic;
using System.Drawing;

namespace ImageResearchNew.Model.ColorModels
{
    public abstract class ColorModel
    {
        public abstract Layer ChannelRotation(List<bool> channelsInfo);
        public abstract List<string> Components { get; }
    }
}
