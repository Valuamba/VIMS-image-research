using Ikc5.TypeLibrary;
using ImageResearch.Engine;
using ImageResearchNew.Helpers;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ImageManagerTools;
using ImageResearchNew.ViewModel.ImageModule;
using ImageResearchNew.ViewModel.ImageModule.ImagePlaceTools;
using ImageResearchNew.ViewModel.ImageModule.ImageProcessors;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ImageResearchNew.ViewModel
{
    public class WorkPageViewModel : BaseNotifyPropertyChanged
    {
        private ObservableCollection<ImagePlaceViewModel> _imagesViewModels;
        public ObservableCollection<ImagePlaceViewModel> ImagesViewModels
        {
            get => _imagesViewModels;
            set => SetProperty(ref _imagesViewModels, value);
        }

        private ImagePlaceViewModel _selectedImageViewModel;
        public ImagePlaceViewModel SelectedImageViewModel
        {
            get => _selectedImageViewModel;
            set
            {
                SetProperty(ref _selectedImageViewModel, value);
                Update();
            }
        }

        public void Update()
        {
            ImageViewOptionsViewModel = new ImageViewOptionsViewModel(SelectedImageViewModel?.imageInfo);
            ImageProcessorBlockViewModel = new ImageProcessorBlockViewModel(SelectedImageViewModel?.imageInfo);
            ImagePlaceToolsBlockViewModel = new ImagePlaceToolsBlockViewModel(SelectedImageViewModel?.imageInfo);

            OnPropertyChanged(nameof(ImageViewOptionsViewModel));
            OnPropertyChanged(nameof(ImageProcessorBlockViewModel));
            OnPropertyChanged(nameof(ImagePlaceToolsBlockViewModel));
        }

        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand RemoveImageCommand { get; }
        public DelegateCommand InteractWithSelectedImagesCommand { get; }
        public DelegateCommand CloseOpenedPaneCommand { get; }

        public ImageProcessorBlockViewModel ImageProcessorBlockViewModel { get; set; }
        public ImagePlaceViewModel ImagePlaceViewModel { get; set; }
        public ImagePlaceToolsBlockViewModel ImagePlaceToolsBlockViewModel { get; set; }
        public ImageViewOptionsViewModel ImageViewOptionsViewModel { get; set; }
        public ImageOscillographViewModel ImageOscillographViewModel { get; set; }

        public WorkPageViewModel()
        {
            ImageProcessorBlockViewModel = new ImageProcessorBlockViewModel(null);
            ImagePlaceToolsBlockViewModel = new ImagePlaceToolsBlockViewModel(null);
            ImageViewOptionsViewModel = new ImageViewOptionsViewModel(null);

            OpenImageCommand = new DelegateCommand((obj) => OpenImage());
            RemoveImageCommand = new DelegateCommand((obj) => RemoveImage());
            InteractWithSelectedImagesCommand = new DelegateCommand((obj) => InteractWithSelectedImages());
            CloseOpenedPaneCommand = new DelegateCommand((obj) => CloseOpenedPane());
            ImagesViewModels = new ObservableCollection<ImagePlaceViewModel>();
        }

        private bool _isPaneOpened;
        public bool IsPaneOpened
        {
            get => _isPaneOpened;
            set => SetProperty(ref _isPaneOpened, value);
        }

        #region CompareView

        private ObservableCollection<ImagePlaceViewModel> _imagesViewModelsToCompare;
        public ObservableCollection<ImagePlaceViewModel> ImagesViewModelsToCompare
        {
            get => _imagesViewModelsToCompare;
            set => SetProperty(ref _imagesViewModelsToCompare, value);
        }

        private IReadOnlyCollection<ImageManagerTool> _managerTools = new List<ImageManagerTool>()
        {
            new CompareImage(),
            new DeleteImage(),
            new CopyImage()
        };
        public IReadOnlyCollection<ImageManagerTool> ManagerTools
        {
            get => _managerTools;
            set => SetProperty(ref _managerTools, value);
        }

        private ImageManagerTool _selectedManagerTool;
        public ImageManagerTool SelectedManagerTool
        {
            get => _selectedManagerTool;
            set => SetProperty(ref _selectedManagerTool, value);
        }

        #endregion

        private void InteractWithSelectedImages()
        {
            SelectedManagerTool?.Invoke(ImagesViewModels, ImagesViewModelsToCompare);
        }

        private void CloseOpenedPane()
        {
            IsPaneOpened = false;
        }

        private void RemoveImage()
        {
            ImagesViewModels.Remove(SelectedImageViewModel);
        }

        private void OpenImage()
        {
            FileInfo file = null;

            if (DialogHelper.ShowOpenFileDialog(out file))
            {
                var image = ImageHelper.Open(file);

                if (image == null)
                {
                    return;
                }

                if(image.Width != 768 || image.Height != 576)
                {
                    var result = MessageBox.Show("У изображения должно быть разрешение 768x576. Желаете интерполировать текущее изображение?", "Неподходящее разрешение", MessageBoxButton.YesNo);
                    switch(result)
                    {
                        case MessageBoxResult.Yes: 
                            image = new ImageFactory(image).Resize(768, 576);
                            break;
                        case MessageBoxResult.No: return;
                    }
                }
                
                var newImageViewModel = new ImagePlaceViewModel(new ImageProcessorInfo(image), file.Name);

                ImagesViewModels.Add(newImageViewModel);
                SelectedImageViewModel = newImageViewModel;
            }
        }
    }
}
