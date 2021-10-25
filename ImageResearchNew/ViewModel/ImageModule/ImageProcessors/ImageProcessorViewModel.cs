using Ikc5.TypeLibrary;
using ImageResearchNew.Model.ImageProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public abstract class ImageProcessorViewModel : BaseNotifyPropertyChanged
    {
        public abstract string Header { get; }
        public abstract ImageProcessor Model { get; set; }
    }
}
