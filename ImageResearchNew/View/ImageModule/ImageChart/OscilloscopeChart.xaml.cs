using ImageResearchNew.Utilities;
using ImageResearchNew.ViewModel.ImageModule.ImageCharts;
using System;
using System.Collections.Generic;
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

namespace ImageResearchNew.View.ImageModule.ImageChart
{
    /// <summary>
    /// Interaction logic for OscilloscopeChart.xaml
    /// </summary>
    public partial class OscilloscopeChart : UserControl
    {
        public OscilloscopeChart()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Plot.ActualModel.InvalidatePlot(true);
        }

        private void ComponentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext != null)
            {
                (DataContext as OscilloscopeChartViewModel).SelectedComponents = ComponentsList.SelectedItems.Cast<string>().ToObservableCollection();
            }
        }
    }
}
