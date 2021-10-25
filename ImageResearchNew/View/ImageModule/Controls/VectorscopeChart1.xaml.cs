using ImageResearch.Engine;
using ImageResearch.Engine.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace ImageResearchNew.View.ImageModule.Controls
{
    /// <summary>
    /// Interaction logic for VectorscopeChart.xaml
    /// </summary>
    public partial class VectorscopeChart : UserControl
    {
        public VectorscopeChart()
        {
            InitializeComponent();
        }

        public Bitmap Source
        {
            get { return (Bitmap)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                    "Source",
                    typeof(Bitmap),
                    typeof(VectorscopeChart),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        private static void OnItemsChangedProperty(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            VectorscopeChart ctrl = (VectorscopeChart)obj;

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

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var rect = new Rect(0, 0, Width, Height);

            if (Source != null)
            {
                drawingContext.DrawImage(DrawCircle(), rect);
            }
        }

        public WriteableBitmap DrawCircle()
        {
            var writeableBitmap = new WriteableBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Bgr32, null);

            double x = Width / 2,
            y = Height / 2;

            unsafe
            {
                using (var bmp = writeableBitmap.GetBitmapContext())
                {
                    writeableBitmap.DrawEllipseCentered((int) Width / 2, (int)Height / 2, (int)Height / 2, (int)Height / 2, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawEllipseCentered((int)Width / 2, (int)Height / 2, (int)Height / 4, (int)Height / 4, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawEllipseCentered((int)Width / 2, (int)Height / 2, (int)Height / 8, (int)Height / 8, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    var vectors = new ImageFactory().GetVectoroscopeOfImage(Source);

                    for (int j = 0; j < vectors.Length; j++)
                    {
                        int rY = vectors[j].RY;
                        int bY = vectors[j].BY;

                        var x2 = (int)x + bY;
                        var y2 = (int)y + rY;

                        if (x2 > 0 && y2 > 0)
                        {
                            var index = y2 * bmp.Width + x2;
                            bmp.Pixels[index] = (128 << 24) | (255 << 16) | (128 << 8) | 128;
                        }
                    }

                    writeableBitmap.DrawLine(0, (int)Height / 2, (int)Width, (int)Height / 2, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawLine((int)Width / 2, 0, (int)Width / 2, (int)Height, System.Windows.Media.Color.FromRgb(255, 0, 0));
                }
            }
            return writeableBitmap;
        }
    }
}
