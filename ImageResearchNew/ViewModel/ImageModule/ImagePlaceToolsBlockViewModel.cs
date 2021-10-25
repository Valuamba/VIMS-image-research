using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ImagePlaceTools;
using System.Collections.Generic;

namespace ImageResearchNew.ViewModel.ImageModule.ImagePlaceTools
{
    public class ImagePlaceToolsBlockViewModel : BaseNotifyPropertyChanged
    {
        private readonly ImageProcessorInfo imageInfo;
        private IReadOnlyCollection<ImagePlaceTool> _imagePlaceTools;

        public DelegateCommand UpdateImageDrawLayers { get; set; }

        public ImagePlaceToolsBlockViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            UpdateImageDrawLayers = new DelegateCommand((obj) => ClearImageDrawingLayers());
            //Здесь баг, в элемент передается родительские элемент
            _imagePlaceTools = new List<ImagePlaceTool>()
            {
                new RegionsTool(imageInfo),
                new RegionInfoTool(imageInfo),
                new FreeHandTool(imageInfo)
            };
        }

        #region Image place tools

        public IReadOnlyCollection<ImagePlaceTool> ImagePlaceTools => _imagePlaceTools;

        private ImagePlaceTool _selectedImagePlaceTool;
        public ImagePlaceTool SelectedImagePlaceTool
        {
            get => _selectedImagePlaceTool;
            set
            {
                SetProperty(ref _selectedImagePlaceTool, value);
                UpdateImagePlaceTool();
            }
        }

        private void UpdateImagePlaceTool()
        {
            if (imageInfo != null)
            {
                imageInfo.ImagePlaceTool = SelectedImagePlaceTool;
            }
        }

        private void ClearImageDrawingLayers()
        {
            if (imageInfo != null)
            {
                imageInfo.DrawLayers.Clear();
                imageInfo.SelectedBitmap = null;
            }
        }

        #endregion
    }
}
