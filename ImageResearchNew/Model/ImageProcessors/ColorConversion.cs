using ImageResearch.Engine;
using ImageResearchNew.Model.ColorModels;
using System;
using System.Collections.Generic;

namespace ImageResearchNew.Model.ImageProcessors
{
    public class ColorConversion : ImageProcessor
    {
		public override Layer ProcessorMethod => conversionMethodFactory[ColorConversionCode];

		private Dictionary<ColorConversionCode, Layer> conversionMethodFactory => new Dictionary<ColorConversionCode, Layer>()
		{
			{ColorConversionCode.YuvToRgb, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Yuv2Bgr)},
			{ColorConversionCode.Yuv400, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Bgr2Yuv, 4, 0, 0)},
			{ColorConversionCode.Yuv411, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Bgr2Yuv, 4, 1, 1)},
			{ColorConversionCode.Yuv420, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Bgr2Yuv, 4, 2, 0)},
			{ColorConversionCode.Yuv422, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Bgr2Yuv, 4, 2, 2)},
			{ColorConversionCode.Yuv444, CreateLayer(Emgu.CV.CvEnum.ColorConversion.Bgr2Yuv, 4, 4, 4)},
		};

		private Dictionary<ColorConversionCode, Func<ColorModel>> colorModelFactory => new Dictionary<ColorConversionCode, Func<ColorModel>>()
		{
			{ColorConversionCode.YuvToRgb, () => new Rgb()},
			{ColorConversionCode.Yuv400, () => new Yuv()},
			{ColorConversionCode.Yuv411, () => new Yuv()},
			{ColorConversionCode.Yuv420, () => new Yuv()},
			{ColorConversionCode.Yuv422, () => new Yuv()},
			{ColorConversionCode.Yuv444, () => new Yuv()},
		};

		private Layer CreateLayer(Emgu.CV.CvEnum.ColorConversion color)
        {
			return new Layer($"Преобразование в {ColorConversionCode}")
			{
				ImageProcessorMethod = (input) => new ImageFactory(input).ConvertTo(color),
				ColorModelMethod = colorModelFactory[ColorConversionCode]
			};
		}

		private Layer CreateLayer(Emgu.CV.CvEnum.ColorConversion color, int a, int b, int j)
		{
			return new Layer($"Преобразование в {ColorConversionCode}")
			{
				ImageProcessorMethod = (input) => new ImageFactory(input).ConvertTo(color, a, b, j),
				ColorModelMethod = colorModelFactory[ColorConversionCode]
			};
		}

		public ColorConversionCode ColorConversionCode { get; set; }
	}
}
