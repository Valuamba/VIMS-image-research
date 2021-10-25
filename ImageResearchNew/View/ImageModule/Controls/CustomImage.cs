using ImageResearchNew.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace ImageResearchNew.View.ImageModule.Controls
{
    public class CustomImage : System.Windows.Controls.Image
    {
        public ObservableCollection<DrawLayer> LayersFactories
        {
            get { return (ObservableCollection<DrawLayer>)GetValue(LayersFactoriesProperty); }
            set { SetValue(LayersFactoriesProperty, value); }
        }

        public static readonly DependencyProperty LayersFactoriesProperty =
            DependencyProperty.Register(
                    "LayersFactories",
                    typeof(ObservableCollection<DrawLayer>),
                    typeof(CustomImage),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        public DrawLayer GridLayer
        {
            get { return (DrawLayer)GetValue(GridLayerProperty); }
            set { SetValue(GridLayerProperty, value); }
        }

        public static readonly DependencyProperty GridLayerProperty =
            DependencyProperty.Register(
                    "GridLayer",
                    typeof(DrawLayer),
                    typeof(CustomImage),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        public static readonly DependencyProperty CellSizeProperty = DependencyProperty.RegisterAttached(
          "CellSize",
          typeof(double),
          typeof(CustomImage),
          new PropertyMetadata(default(double)));

        public double CellSize
        {
            get { return (double)GetValue(CellSizeProperty); }
            set { SetValue(CellSizeProperty, value); }
        }

        private static void OnItemsChangedProperty(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CustomImage ctrl = (CustomImage)obj;

            INotifyCollectionChanged oldList = args.OldValue as INotifyCollectionChanged;
            INotifyCollectionChanged newList = args.NewValue as INotifyCollectionChanged;

            //If the old list implements the INotifyCollectionChanged interface, then unsubscribe to CollectionChanged events.
            if (oldList != null)
                oldList.CollectionChanged -= ctrl.OnItemsCollectionChanged;
            //If the new list implements the INotifyCollectionChanged interface, then subscribe to CollectionChanged events.
            if (newList != null)
                newList.CollectionChanged += ctrl.OnItemsCollectionChanged;
        }

        private void OnItemsCollectionChanged(object source, NotifyCollectionChangedEventArgs args)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            ImageSource imageSource = Source;

            if (imageSource == null)
            {
                return;
            }

            dc.DrawImage(imageSource, new Rect(new System.Windows.Point(), RenderSize));

            if(GridLayer != null && GridLayer.IsHidden == false)
            {
                GridLayer.Method(dc);
            }

            if (LayersFactories != null && LayersFactories.Count != 0)
            {
                for (int i = 0; i < LayersFactories.Count; i++)
                {
                    if(!LayersFactories[i].IsHidden)
                    {
                        LayersFactories[i].Method(dc);
                    }
                }
            }
        }
    }
}

