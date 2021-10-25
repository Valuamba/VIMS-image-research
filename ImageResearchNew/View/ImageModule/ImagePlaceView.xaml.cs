using ImageResearchNew.Helpers;
using ImageResearchNew.Model.ImagePlaceTools;
using ImageResearchNew.Utilities;
using ImageResearchNew.View.ImageModule.Controls;
using ImageResearchNew.ViewModel.ImageModule;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageResearchNew.View.ImageModule
{
    /// <summary>
    /// Interaction logic for ImagePlaceView.xaml
    /// </summary>
    /// 
    public partial class ImagePlaceView : UserControl
    {
        public ImagePlaceView()
        {
            InitializeComponent();
        }

        public ImagePlaceTool ImagePlaceTool => (DataContext as ImagePlaceViewModel).ImagePlaceTool;

        private void EditedImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImagePlaceTool?.OnMouseDown(e.GetPosition(sender as CustomImage));
        }

        private void EditedImage_MouseMove(object sender, MouseEventArgs e)
        {
            ImagePlaceTool?.OnMouseMove(e.GetPosition(sender as CustomImage));
        }

        private void EditedImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImagePlaceTool?.OnMouseUp(e.GetPosition(sender as CustomImage));
        }
    }
}
