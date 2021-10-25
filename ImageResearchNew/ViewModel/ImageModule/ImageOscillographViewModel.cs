using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ColorModels;
using ImageResearchNew.ViewModel.ImageModule.ImageCharts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ImageResearchNew.ViewModel.ImageModule
{
    public class ImageOscillographViewModel : BaseNotifyPropertyChanged
    {
        public VectorscopeChartViewModel VectorscopeChartViewModel { get; }
        public HistogramChartViewModel HistogramChartViewModel { get; }
        public OscilloscopeChartViewModel OscilloscopeChartViewModel { get; }

        public DelegateCommand ChangeViewingComponentCommand { get; set; }

        public ImageOscillographViewModel(ImageProcessorInfo imageInfo)
        {
            VectorscopeChartViewModel = new VectorscopeChartViewModel(imageInfo);
            HistogramChartViewModel = new HistogramChartViewModel(imageInfo);
            OscilloscopeChartViewModel = new OscilloscopeChartViewModel(imageInfo);
        }
    }
}
