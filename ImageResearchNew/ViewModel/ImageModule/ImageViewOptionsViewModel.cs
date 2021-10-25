using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.View.ImageModule.Windows;
using ImageResearchNew.ViewModel.ImageModule.ImageViewOptions;
namespace ImageResearchNew.ViewModel.ImageModule
{
    public class ImageViewOptionsViewModel : BaseNotifyPropertyChanged
    {
        private readonly ImageProcessorInfo imageInfo;

        public DelegateCommand OpenOscillographWindow { get; set; }

        public ImageOscillographViewModel ImageOscillographViewModel { get; }

        public ImageViewOptionsViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            if (imageInfo != null)
            {
                GridOptionsViewModel = new GridOptionsViewModel(imageInfo.GridOptions);
                OpenOscillographWindow = new DelegateCommand((obj) => ShowOscillographWindow());
                ImageOscillographViewModel = new ImageOscillographViewModel(imageInfo);
            }
        }

        private void ShowOscillographWindow()
        {
            var oscillographWindow = new OscilloscopeWindow();
            oscillographWindow.DataContext = ImageOscillographViewModel;
            oscillographWindow.Show();
        }

        public GridOptionsViewModel GridOptionsViewModel { get; }
    }
}
