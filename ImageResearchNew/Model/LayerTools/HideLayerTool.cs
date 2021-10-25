using System.Collections.ObjectModel;
using System.Linq;

namespace ImageResearchNew.Model.LayerTools
{
    public class HideLayerTool : LayerTool
    {
        public override void Invoke(ObservableCollection<Layer> layers, Layer selectedLayer)
        {
            if (selectedLayer != null)
            {
                var layer = layers.Single(s => s.Equals(selectedLayer));
                layer.IsHidden = !layer.IsHidden;
            }
        }
    }
}
