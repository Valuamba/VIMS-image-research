using ImageResearchNew.ViewModel.ImageModule;
using System.Collections.ObjectModel;

namespace ImageResearchNew.Model.ImageManagerTools
{
    public class DeleteImage : ImageManagerTool
    {
        public override void Invoke(ObservableCollection<ImagePlaceViewModel> images, ObservableCollection<ImagePlaceViewModel> selectedImages)
        {
            for (int i = 0; i < selectedImages.Count; i++)
            {
                images?.Remove(selectedImages[i]);
            }
        }
    }
}
