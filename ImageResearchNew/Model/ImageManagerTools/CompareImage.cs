using ImageResearchNew.AppWindows;
using ImageResearchNew.ViewModel;
using ImageResearchNew.ViewModel.ImageModule;
using System.Collections.ObjectModel;

namespace ImageResearchNew.Model.ImageManagerTools
{
    public class CompareImage : ImageManagerTool
    {
        public override void Invoke(ObservableCollection<ImagePlaceViewModel> images, ObservableCollection<ImagePlaceViewModel> selectedImages)
        {
            var window = new CompareWindow();
            var obj = new DynamicGridViewModel(selectedImages);
            window.DataContext = obj;
            window.Show();
        }
    }
}
