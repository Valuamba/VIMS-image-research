using Ikc5.TypeLibrary;
using ImageResearch.Engine;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ColorModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageResearchNew.ViewModel.ImageModule.ImageCharts
{
    public class OscilloscopeChartViewModel : BaseNotifyPropertyChanged
    {
        public ColorModel ColorModel
        {
            get => imageInfo.ColorModel;
        }

        public Bitmap ModifiedImage
        {
            get => imageInfo.SelectedBitmap ?? imageInfo.ModifiedImage;
        }

        private int _rowNumber = 1;
        public int RowNumber
        {
            get => _rowNumber;
            set
            {
                SetProperty(ref _rowNumber, value);
                UpdateSeries();
            }
        }

        private readonly ImageProcessorInfo imageInfo;

        public DelegateCommand MovePlotCommand { get; set; }

        public OscilloscopeChartViewModel(ImageProcessorInfo imageInfo)
        {
            this.imageInfo = imageInfo;
            MovePlotCommand = new DelegateCommand((obj) => OnMoveCommand());
            Ch1 = new ObservableCollection<DataPoint>();
            Ch2 = new ObservableCollection<DataPoint>();
            Ch3 = new ObservableCollection<DataPoint>();
        }

        private int _minimum = 10;
        public int Minimum
        {
            get => _minimum;
            set => SetProperty(ref _minimum, value);
        }

        private int _maximum = 50;
        public int Maximum
        {
            get => _maximum;
            set => SetProperty(ref _maximum, value);
        }

        private void OnMoveCommand()
        {
            Minimum += 10;
            Maximum += 10;
        }

        private ObservableCollection<string> _selectedComponents;
        public ObservableCollection<string> SelectedComponents
        {
            get => _selectedComponents;
            set
            {
                SetProperty(ref _selectedComponents, value);
                UpdateSeries();
            }
        }

        public List<string> Channels => ColorModel.Components;

        private ObservableCollection<DataPoint> _ch1;
        private ObservableCollection<DataPoint> _ch2;
        private ObservableCollection<DataPoint> _ch3;

        public ObservableCollection<DataPoint> Ch1
        {
            get => _ch1;
            set => SetProperty(ref _ch1, value);
        }

        public ObservableCollection<DataPoint> Ch2
        {
            get => _ch2;
            set => SetProperty(ref _ch2, value);
        }

        public ObservableCollection<DataPoint> Ch3
        {
            get => _ch3;
            set => SetProperty(ref _ch3, value);
        }

        public void UpdateSeries()
        {
            Ch1.Clear();
            Ch2.Clear();
            Ch3.Clear();
            for(int i = 0; i < SelectedComponents.Count; i++)
            {
                var index = Channels.IndexOf(SelectedComponents[i]);
                switch (index)
                {
                    case 0: Ch1 = CalculatePointsForChannel(new ImageFactory(ModifiedImage).GetChannel(index)); break;
                    case 1: Ch2 = CalculatePointsForChannel(new ImageFactory(ModifiedImage).GetChannel(index)); break;
                    case 2: Ch3 = CalculatePointsForChannel(new ImageFactory(ModifiedImage).GetChannel(index)); break;
                }
            }
        }

        private Dictionary<string, OxyColor> ChannelColorFactory = new Dictionary<string, OxyColor>()
        {
            {"R", OxyColor.FromRgb(255, 0, 0) },
            {"G", OxyColor.FromRgb(0, 255, 0) },
            {"B", OxyColor.FromRgb(0, 0, 255) },
        };

        public ObservableCollection<DataPoint> CalculatePointsForChannel(Bitmap bmp)
        {
            var list = new List<int>();
            var width = bmp.Width;
            var bitmap = bmp.Clone(new Rectangle(0, RowNumber, width, 1), PixelFormat.Format8bppIndexed);
            var bitData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            var height = bitmap.Height;

            unsafe
            {
                var ptr = (byte*)bitData.Scan0;

                for (int i = 0; i < height; i++)
                {
                    var ptr2 = ptr;
                    for (int j = 0; j < width; j++, ptr2++)
                    {
                        list.Add(ptr2[0]);
                    }
                    ptr += bitData.Stride;
                }
            }

            bitmap.UnlockBits(bitData);

            var points = new ObservableCollection<DataPoint>();
            for (int i = 0; i < list.Count; i++)
            {
                points.Add(new DataPoint(i, list[i]));
            }

            return points;
        }
    }
}
