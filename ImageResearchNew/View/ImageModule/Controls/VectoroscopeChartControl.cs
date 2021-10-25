using ImageResearch.Engine;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageResearchNew.View.ImageModule.Controls
{
    public class VectoroscopeChartControl : Control
    {
        public Bitmap Source
        {
            get { return (Bitmap)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public string XAsixText
        {
            get { return (string)GetValue(XAsixTextProperty); }
            set { SetValue(XAsixTextProperty, value); }
        }

        public string YAsixText
        {
            get { return (string)GetValue(YAsixTextProperty); }
            set { SetValue(YAsixTextProperty, value); }
        }

        public static readonly DependencyProperty XAsixTextProperty =
            DependencyProperty.Register(
                    "XAsixText",
                    typeof(string),
                    typeof(VectoroscopeChartControl),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        public static readonly DependencyProperty YAsixTextProperty =
            DependencyProperty.Register(
                    "YAsixText",
                    typeof(string),
                    typeof(VectoroscopeChartControl),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                    "Source",
                    typeof(Bitmap),
                    typeof(VectoroscopeChartControl),
                    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsChangedProperty));

        private static void OnItemsChangedProperty(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            VectoroscopeChartControl ctrl = (VectoroscopeChartControl)obj;

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

                drawingContext.DrawText(new FormattedText(XAsixText, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"), 14,
                new SolidColorBrush(Colors.Red), 96), new System.Windows.Point((Width - 6) / 2, 0));

                drawingContext.DrawText(new FormattedText(YAsixText, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"), 14,
                new SolidColorBrush(Colors.Red), 96), new System.Windows.Point(Width - 30, (Height - 10) / 2));
            }
        }

        private const int W1 = 227 * 2;
        private const int H2 = 180 * 2;


        public WriteableBitmap DrawCircle()
        {
            var writeableBitmap = new WriteableBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Bgr32, null);

            int radiusX = 230;
            int radiusY = 190;

            int xc = (int) Width / 2;
            int yc = (int) Height / 2;

            unsafe
            {
                using (var bmp = writeableBitmap.GetBitmapContext())
                {
                    writeableBitmap.DrawRectangle(0, 0, (int)Width, (int)Height, System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
                    writeableBitmap.DrawEllipseCentered(xc, yc, radiusX, radiusY, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawEllipseCentered(xc, yc, radiusX / 2, radiusY / 2, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawEllipseCentered(xc, yc, radiusX / 4, radiusY / 4, System.Windows.Media.Color.FromRgb(255, 0, 0));

                    var vectors = new ImageFactory().GetVectoroscopeOfImage(Source);

                    for (int j = 0; j < vectors.Length; j++)
                    {
                        int rY = vectors[j].RY;
                        int bY = vectors[j].BY;

                        var x2 = (int)xc + bY;
                        var y2 = (int)yc + rY;

                        if (x2 > 0 && y2 > 0)
                        {
                            var index = y2 * bmp.Width + x2;
                            bmp.Pixels[index] = (128 << 24) | (255 << 16) | (128 << 8) | 128;
                        }
                    }
                    writeableBitmap.DrawLine(xc - radiusX, yc, xc + radiusX, yc, System.Windows.Media.Color.FromRgb(255, 0, 0));
                    writeableBitmap.DrawLine(xc, yc - radiusY, xc, yc + radiusY, System.Windows.Media.Color.FromRgb(255, 0, 0));
                }
            }
            return writeableBitmap;
        }
    }
}
