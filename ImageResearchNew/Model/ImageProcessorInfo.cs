using ImageResearchNew.Model.ColorModels;
using ImageResearchNew.Model.ImagePlaceTools;
using ImageResearchNew.Model.ViewOptions;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ImageResearchNew.Model
{
    public class ImageProcessorInfo : ICloneable
	{
		public GridOptions GridOptions { get; }

		public string Name { get; set; }

		private Bitmap _sourceImage;
		public Bitmap SourceImage
		{
			get => _sourceImage;
			set
			{
				_sourceImage = value;
				UpdateModifiedImage();
			}
		}

		public void UpdateModifiedImage()
        {
			ModifiedImage = SourceImage;
			if(ImageProcessors.Count == 0)
            {
				ColorModel = new Rgb();
			}
			else foreach(var imageProcessor in ImageProcessors)
            {
				if (!imageProcessor.IsHidden)
				{
					ModifiedImage = imageProcessor.ImageProcessorMethod(ModifiedImage);
					if (imageProcessor.ColorModelMethod != null)
					{
						ColorModel = imageProcessor.ColorModelMethod();
					}
				}
            }
		}

		private Bitmap _modifiedImage;
		public Bitmap ModifiedImage
		{
			get => _modifiedImage;
			set
			{
				_modifiedImage = value;
				ChangeModifiedImage?.Invoke();
			}
		}

		public Bitmap ViewingImage
		{
			get => ViewImageLayer?.ImageProcessorMethod(_modifiedImage) ?? _modifiedImage;
		}

		private Bitmap _selectedBitmap;
		public Bitmap SelectedBitmap
		{
			get => _selectedBitmap;
			set
			{
				_selectedBitmap = value;
			}
		}

		private ColorModel _colorModel;
		public ColorModel ColorModel 
		{ 
			get => _colorModel;
			set
			{
				_colorModel = value;
				ChangeColorModel?.Invoke();
			}
		}

		private Layer _viewImageLayer;
		/// <summary>
		/// This layer need for displaying ModifiedImage
		/// </summary>
		public Layer ViewImageLayer 
		{ 
			get => _viewImageLayer;
			set
			{
				_viewImageLayer = value;
				ChangeViewLayer?.Invoke();
			}
		}

		public ImagePlaceTool ImagePlaceTool { get; set; }

		public ObservableCollection<Layer> ImageProcessors { get; set; }

		public ObservableCollection<DrawLayer> DrawLayers { get; set; }

        public ImageProcessorInfo(Bitmap bitmap)
        {
			DrawLayers = new ObservableCollection<DrawLayer>();
			ImageProcessors = new ObservableCollection<Layer>();
            ImageProcessors.CollectionChanged += ImageProcessors_CollectionChanged1;
			ColorModel = new Rgb();
			SourceImage = bitmap;
			ModifiedImage = SourceImage;
			GridOptions = new GridOptions(ModifiedImage.Width, ModifiedImage.Height);
		}

        private void ImageProcessors_CollectionChanged1(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
			UpdateModifiedImage();
		}

        private void ImageProcessors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
			Notify?.Invoke();
		}

		public void UpdateImageViewing()
        {
			ModifiedImage = ViewImageLayer.ImageProcessorMethod(ModifiedImage);
		}

		public void OnModifiedImageChanged()
        {
			ChangeModifiedImage?.Invoke();
		}

        public object Clone()
        {
			return this.MemberwiseClone();
        }

        public int GridSize { get; set; } = 30;

		public event Action ChangeModifiedImage;
		public event Action ChangeViewLayer;
		public event Action ChangeColorModel;

		public event Action Notify;
	}
}
