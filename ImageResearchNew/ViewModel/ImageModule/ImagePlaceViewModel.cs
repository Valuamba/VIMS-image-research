using Ikc5.TypeLibrary;
using ImageResearch.Engine;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ColorModels;
using ImageResearchNew.Model.ImagePlaceTools;
using ImageResearchNew.ViewModel.ImageModule.Channels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace ImageResearchNew.ViewModel.ImageModule
{
    public class ImagePlaceViewModel : BaseNotifyPropertyChanged
    {
        public string Name { get; set; }

        public Bitmap ViewingImage
        {
            get => imageInfo.ViewingImage;
        }

        public Bitmap ModifiedImage
        {
            get => imageInfo.ModifiedImage;
        }

        public Bitmap SourceImage
        {
            get
            {
                if (imageInfo.ColorModel is Rgb)
                {
                    return imageInfo.ViewImageLayer?.ImageProcessorMethod(imageInfo.SourceImage) ?? imageInfo.SourceImage;
                }

                return imageInfo.SourceImage;
            }
        }

        public ImagePlaceTool ImagePlaceTool => imageInfo.ImagePlaceTool;

        public ImageProcessorInfo imageInfo { get; set; }
        
        public ChannelsViewModel ModifiedImageViewModel { get; set; }
        public ChannelsViewModel SourceImageViewModel { get; set; }

        public ImagePlaceViewModel(ImageProcessorInfo imageInfo, string name) 
        {
            this.imageInfo = imageInfo;

            Name = name;
            if (imageInfo != null)
            {
                ModifiedImageViewModel = new ChannelsViewModel(imageInfo);

                imageInfo.ChangeModifiedImage += () => OnPropertyChanged(nameof(ViewingImage));
                imageInfo.ChangeViewLayer += () => OnPropertyChanged(nameof(ViewingImage));
                imageInfo.ChangeViewLayer += () => OnPropertyChanged(nameof(SourceImage));
                imageInfo.GridOptions.ChangeGridOprtionProperty += () => OnPropertyChanged(nameof(GridLayer));
                imageInfo.DrawLayers.CollectionChanged +=
                    (object sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(AppliedDrawingLayers));
            }
        }

        public DrawLayer GridLayer => imageInfo.GridOptions.GridDrawLayer;

        public ObservableCollection<DrawLayer> AppliedDrawingLayers => imageInfo.DrawLayers;
    }
}
