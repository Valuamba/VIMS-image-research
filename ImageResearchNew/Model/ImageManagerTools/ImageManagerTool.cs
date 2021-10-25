using ImageResearchNew.ViewModel.ImageModule;
using System.Collections.ObjectModel;

namespace ImageResearchNew.Model.ImageManagerTools
{
    public abstract class ImageManagerTool
    {
        public abstract void Invoke(ObservableCollection<ImagePlaceViewModel> images, ObservableCollection<ImagePlaceViewModel> selectedImages);
    }
}
