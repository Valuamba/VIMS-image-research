using ImageResearch.Engine;

namespace ImageResearchNew.Model.ImageProcessors
{
    public class ImageQuantization : ImageProcessor
    {
        public int Levels { get; set; }
        public override Layer ProcessorMethod => new Layer((input) => new ImageFactory(input).Quant(input, Levels), $"Разрядность квантования: {Levels}");
    }
}
