using Ikc5.TypeLibrary;
using ImageResearch.Engine;
using ImageResearchNew.Model;
using LiveCharts;
using System.Drawing;

namespace ImageResearchNew.ViewModel.ImageModule.ImageCharts
{
    public class HistogramChartViewModel : BaseNotifyPropertyChanged
    {
        public Bitmap ModifiedImage
        {
            get => imageInfo.SelectedBitmap ?? imageInfo.ModifiedImage;
        }

        private readonly ImageProcessorInfo imageInfo;

        public HistogramChartViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            imageInfo.ChangeModifiedImage += () => OnPropertyChanged(nameof(ModifiedImage));
            imageInfo.ChangeModifiedImage += () => OnPropertyChanged(nameof(LuminanceGroup));
        }

        public ChartValues<int> LuminanceGroup
        {
            get => new ChartValues<int>(new ImageFactory().BuildHistogram(ModifiedImage));
        }

        public int ColorCount
        {
            get => 255;
        }
    }
}
