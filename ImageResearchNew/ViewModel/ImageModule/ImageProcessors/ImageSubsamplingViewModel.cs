using ImageResearchNew.Model.ImageProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public class ImageSubsamplingViewModel : ImageProcessorViewModel
    {
        public override string Header => "Subsampling";

        public override ImageProcessor Model { get; set; }

        public ImageSubsamplingViewModel()
        {
            Model = new ImageSubsampling();
            Value = SubsamplingValues.ElementAt(0);
        }

        private IReadOnlyCollection<int> _subsamplingValues = new List<int>()
        {
            2,
            4,
            6,
            8,
            16
        };
        public IReadOnlyCollection<int> SubsamplingValues => _subsamplingValues;

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                UpdateSubsamplingMethod();
            }
        }

        private bool _isShouldResizeToSource = false;
        public bool IsShouldResizeToSource
        {
            get => _isShouldResizeToSource;
            set
            {
                SetProperty(ref _isShouldResizeToSource, value);
                UpdateSubsamplingMethod();
            }
        }

        public void UpdateSubsamplingMethod()
        {
            (Model as ImageSubsampling).N = Value;
            (Model as ImageSubsampling).ResizeToSource = IsShouldResizeToSource;
        }
    }
}
