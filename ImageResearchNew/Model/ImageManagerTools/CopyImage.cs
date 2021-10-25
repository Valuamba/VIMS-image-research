using ImageResearchNew.ViewModel.ImageModule;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageResearchNew.Model.ImageManagerTools
{
    public class CopyImage : ImageManagerTool
    {
        public override void Invoke(ObservableCollection<ImagePlaceViewModel> images, ObservableCollection<ImagePlaceViewModel> selectedImages)
        {
            if (selectedImages != null)
            {
                for (int i = 0; i < selectedImages.Count; i++)
                {
                    var equalsImages = images.Where(vm => vm.Name.StartsWith(Path.GetFileNameWithoutExtension(selectedImages[i].Name)));
                    var sourceName = images.First(s => s.Name.Length == equalsImages.Min(j => j.Name.Length));
                    var name = equalsImages.Count() == 0
                        ? selectedImages[i].Name
                        : $"{Path.GetFileNameWithoutExtension(sourceName.Name)}_{equalsImages.Count() + 1}{Path.GetExtension(sourceName.Name)}";
                    var newViewModel = new ImagePlaceViewModel(new ImageProcessorInfo((Bitmap)selectedImages[i].imageInfo.ModifiedImage.Clone()), name);

                    images.Add(newViewModel);
                }
            }
        }
    }
}
