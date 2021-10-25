using System.Collections.ObjectModel;

namespace ImageResearchNew.Model.LayerTools
{
    public abstract class LayerTool
    {
        public abstract void Invoke(ObservableCollection<Layer> layers, Layer selectedLayer);
    }
}
