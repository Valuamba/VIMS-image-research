using ImageResearchNew.Model;
using ImageResearchNew.Model.ColorModels;
using ImageResearchNew.Utilities;
using ImageResearchNew.ViewModel.ImageModule.Channels;
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

namespace ImageResearchNew.View.ImageModule.Channels
{
    /// <summary>
    /// Interaction logic for ChannelsViewControl.xaml
    /// </summary>
    public partial class ChannelsViewControl : UserControl
    {
        public ChannelsViewControl()
        {
            InitializeComponent();
        }

        private void ComponentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext != null)
            {
                (DataContext as ChannelsViewModel).SelectedComponents = ComponentsList.SelectedItems.Cast<string>().ToObservableCollection();
            }
        }
    }
}
