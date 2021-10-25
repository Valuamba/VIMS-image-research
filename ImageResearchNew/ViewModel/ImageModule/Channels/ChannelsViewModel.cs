using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ColorModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.Channels
{
    public class ChannelsViewModel : BaseNotifyPropertyChanged
    {
        private ObservableCollection<string> _selectedComponents;
        public ObservableCollection<string> SelectedComponents
        {
            get => _selectedComponents;
            set
            {
                _selectedComponents = value;
                ChangeViewingComponent();
            }
        }

        private readonly ImageProcessorInfo imageInfo;

        public DelegateCommand ChangeViewingComponentCommand { get; set; }

        public ChannelsViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            //imageInfo.ChangeModifiedImage += () => SelectedComponents = null;
            ChangeViewingComponentCommand = new DelegateCommand((obj) => ChangeViewingComponent());
            //imageInfo.ChangeModifiedImage += () => SelectedComponents = null;
            //imageInfo.ChangeModifiedImage += () => OnPropertyChanged(nameof(ViewingImage));

            imageInfo.ChangeColorModel += () => OnPropertyChanged(nameof(ColorModel));
            imageInfo.ChangeColorModel += () => OnPropertyChanged(nameof(Channels));
            imageInfo.ChangeColorModel += () => SelectedComponents = new ObservableCollection<string>() { Channels[0] };

        }

        public ColorModel ColorModel
        {
            get => imageInfo.ColorModel;
        }

        public List<string> Channels => ColorModel?.Components;

        public void ChangeViewingComponent()
        {
            if (SelectedComponents != null && SelectedComponents.Count >= 0 && ColorModel != null)
            {
                var channelRotationPattern = new List<bool>(Channels.Count) { false, false, false };

                for (int i = 0; i < Channels.Count; i++)
                {
                    channelRotationPattern[i] = SelectedComponents.IndexOf(Channels[i]) != -1 ? true : false;
                }
                imageInfo.ViewImageLayer = ColorModel.ChannelRotation(channelRotationPattern);
            }
        }
    }
}
