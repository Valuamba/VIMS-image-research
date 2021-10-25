using ImageResearch.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace ImageResearchNew.Model.ColorModels
{
    public class Yuv : ColorModel
    {
        public override List<string> Components => new List<string>()
        {
            "Y", "U", "V"
        };

        public override Layer ChannelRotation(List<bool> channelsInfo)
        {
            return new Layer((input) => new ImageFactory(input).ChannelYUVRotation(channelsInfo));
        }
    }
}
