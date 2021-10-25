using ImageResearchNew.Utilities;
using ImageResearchNew.ViewModel;
using ImageResearchNew.ViewModel.ImageModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageResearchNew.View.ImageModule
{
    /// <summary>
    /// Interaction logic for ImageManagerControl.xaml
    /// </summary>
    public partial class ImageManagerControl : UserControl
    {
        public ImageManagerControl()
        {
            InitializeComponent();
        }

        private void OnImageCompareSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as WorkPageViewModel).ImagesViewModelsToCompare = ImageToCompareList.SelectedItems.Cast<ImagePlaceViewModel>().ToObservableCollection();
        }
    }
}
