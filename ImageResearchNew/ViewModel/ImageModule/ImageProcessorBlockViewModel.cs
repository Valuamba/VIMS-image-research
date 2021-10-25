using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.Model.LayerTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public class ImageProcessorBlockViewModel : BaseNotifyPropertyChanged
	{
		public DelegateCommand UpdateTragetImage { get; set; }
		public DelegateCommand ApplyImageProcessor { get; set; }
		public DelegateCommand IteractWithAppliedLayers { get; set; }

		private readonly ImageProcessorInfo imageInfo;

		public ImageProcessorBlockViewModel(ImageProcessorInfo imageInfo)
		{
			this.imageInfo = imageInfo; 
            AppliedImageProcessors = imageInfo?.ImageProcessors;
            ApplyImageProcessor = new DelegateCommand(obj => AddImageProcessor());
			IteractWithAppliedLayers = new DelegateCommand(obj => UpdateAppliedLayers());
			SelectedViewModelProcessorViewModel = ImageProcessorViewModels.ElementAt(0);
		}

        #region Image Block Processor
        private void AddImageProcessor()
        {
			AppliedImageProcessors?.Add(SelectedViewModelProcessorViewModel.Model.ProcessorMethod);
		}

		private IReadOnlyCollection<ImageProcessorViewModel> _imageProcessorViewModels = new List<ImageProcessorViewModel>()
		{
			new ColorConversionViewModel(),
			new ImageConvolutionViewModel(),
			new ImageSubsamplingViewModel(),
			new ImageQuantizationViewModel()
		};
		public IReadOnlyCollection<ImageProcessorViewModel> ImageProcessorViewModels => _imageProcessorViewModels;

		public ImageProcessorViewModel _selectedViewModelProcessorViewModel;
		public ImageProcessorViewModel SelectedViewModelProcessorViewModel
		{
			get => _selectedViewModelProcessorViewModel;
			set => SetProperty(ref _selectedViewModelProcessorViewModel, value);
		}
        #endregion

        #region Layer

        private ObservableCollection<Layer> _appliedImageProcessors;
		public ObservableCollection<Layer> AppliedImageProcessors
		{
			get => _appliedImageProcessors;
			set => SetProperty(ref _appliedImageProcessors, value);
		}

		private Layer _selectedLayer;
		public Layer SelectedLayer
        {
			get => _selectedLayer;
			set => SetProperty(ref _selectedLayer, value);
        }

        #endregion

        #region Layer Tools

        private IReadOnlyCollection<LayerTool> _layerTools = new ObservableCollection<LayerTool>()
		{
			new DeleteLayerTool(),
			new HideLayerTool()
		};
		public IReadOnlyCollection<LayerTool> LayerTools => _layerTools;

		private LayerTool _selectedLayerTool;
		public LayerTool SelectedLayerTool
        {
			get => _selectedLayerTool;
			set => SetProperty(ref _selectedLayerTool, value);
        }

		public void UpdateAppliedLayers()
        {
			var count = AppliedImageProcessors.Count;
			SelectedLayerTool?.Invoke(AppliedImageProcessors, SelectedLayer);
			if (count == AppliedImageProcessors.Count)
			{
				imageInfo.UpdateModifiedImage();
			}
		}

		#endregion
	}
}
