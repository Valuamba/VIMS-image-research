using ImageResearch.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace ImageResearchNew.Model.ColorModels
{
    public class Rgb : ColorModel
    {
        public override List<string> Components => new List<string>()
        {
            "R", "G", "B"
        };

        public override Layer ChannelRotation(List<bool> channelsInfo)
        {
            return new Layer((input) => new ImageFactory(input).ChannelRotation(channelsInfo));
        }        
    }
}
