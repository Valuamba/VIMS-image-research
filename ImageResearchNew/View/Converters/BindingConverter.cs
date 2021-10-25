using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageResearchNew.View.Converters
{
    public class PropertyConverter
    {
        public object Source { get; set; }
        public Binding PropertyBinding { get; set; }
    }

    public class BindingConverter : List<PropertyConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Type typedValue))
                throw new InvalidCastException();

            return Convert(typedValue);
        }

        public DataTemplate Convert(Type value)
        {
            if (value == null)
                return new DataTemplate();

            var visualTree = new FrameworkElementFactory(value);

            foreach (var propertyConverter in this)
            {
                var descriptor = DependencyPropertyDescriptor.FromName(
                   propertyConverter.Source.ToString(),
                   value,
                   value);
                if (descriptor != null)
                {
                    var property = descriptor.DependencyProperty;
                    visualTree.SetBinding(property, propertyConverter.PropertyBinding);
                }
            }

            return new DataTemplate
            {
                VisualTree = visualTree
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
