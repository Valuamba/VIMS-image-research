using ImageResearch.Engine;

namespace ImageResearchNew.Model.ImageProcessors
{
    public class ImageConvolution : ImageProcessor
    {
        public float[,] Kernel { get; set; }

        public override Layer ProcessorMethod  => new Layer((input) => new ImageFactory(input).ApplyFilter(Kernel), $"Корреляция маской {Kernel.GetLength(0)}x{Kernel.GetLength(0)}");
    }
}
