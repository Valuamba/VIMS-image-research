using Ikc5.TypeLibrary;
using ImageResearch.Engine;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ImageProcessors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public class ColorConversionViewModel : ImageProcessorViewModel
	{
		public override ImageProcessor Model { get; set; }

		public ColorConversionViewModel()
		{
			Model = new ColorConversion();
			SelectedColorCode = ColorConversionCodes[0];
		}

		private List<ColorConversionCode> _colorConversionCodes = new List<ColorConversionCode>()
		{
			ColorConversionCode.YuvToRgb,
			ColorConversionCode.Yuv400,
			ColorConversionCode.Yuv411,
			ColorConversionCode.Yuv420,
			ColorConversionCode.Yuv422,
			ColorConversionCode.Yuv444,
		};

		public List<ColorConversionCode> ColorConversionCodes
		{
			get => _colorConversionCodes;
		}

        private ColorConversionCode _selectedColorCode;
        public ColorConversionCode SelectedColorCode
        {
            get => _selectedColorCode;
            set
            {
                SetProperty(ref _selectedColorCode, value);
                UpdateImageFactory();
            }
        }

        public override string Header => "Color Conversion";

		public void UpdateImageFactory()
        {
			(Model as ColorConversion).ColorConversionCode = SelectedColorCode;
        }
    }
}
