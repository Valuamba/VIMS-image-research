using ImageResearch.Engine;

namespace ImageResearchNew.Model.ImageProcessors
{
    public class ImageSubsampling : ImageProcessor
    {
        public int N { get; set; }
        public bool ResizeToSource { get; set; }
        public override Layer ProcessorMethod => new Layer((input) => new ImageFactory(input).Subsampling(N, ResizeToSource), $"Прореживание коэффициентом: {N}");
    }
}
