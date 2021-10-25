using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using System.Drawing;

namespace ImageResearchNew.ViewModel.ImageModule.ImageCharts
{
    public class VectorscopeChartViewModel : BaseNotifyPropertyChanged
    {
        public string YAxisText => "B-Y";
        public string XAxisText => "R-Y";

        public Bitmap ModifiedImage
        {
            get => imageInfo.SelectedBitmap ?? imageInfo.ModifiedImage;
        }

        private readonly ImageProcessorInfo imageInfo;

        public VectorscopeChartViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            imageInfo.ChangeModifiedImage += () => OnPropertyChanged(nameof(ModifiedImage));
        }
    }
}
