using ImageResearchNew.Model.ImageProcessors;
using System.Collections.Generic;
using System.Linq;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public class ImageQuantizationViewModel : ImageProcessorViewModel
    {
        public ImageQuantizationViewModel()
        {
            Model = new ImageQuantization();
            SelectedLevel = QuantizeLevels.ElementAt(0);
        }

        private int _selectedLevel;
        public int SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                SetProperty(ref _selectedLevel, value);
                UpdateQuantizationLevels();
            }
        }

        private IReadOnlyCollection<int> _quantizeLevels = new List<int>()
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8
        };
        public IReadOnlyCollection<int> QuantizeLevels => _quantizeLevels;

        public override string Header => "Quantization";

        public override ImageProcessor Model { get; set; }

        public void UpdateQuantizationLevels()
        {
            (Model as ImageQuantization).Levels = SelectedLevel;
        }
    }
}
