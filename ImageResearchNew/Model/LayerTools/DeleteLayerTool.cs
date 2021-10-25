using System.Collections.ObjectModel;

namespace ImageResearchNew.Model.LayerTools
{
    public class DeleteLayerTool : LayerTool
    {
        public override void Invoke(ObservableCollection<Layer> layers, Layer selectedLayer)
        {
            layers?.Remove(selectedLayer);
        }
    }
}
