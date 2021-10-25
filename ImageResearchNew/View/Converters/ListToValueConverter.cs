using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ImageResearchNew.View.Converters
{
	public abstract class ListToValueConverter<TIn, TOut> : List<TIn>, IValueConverter
	{
		public abstract TOut Convert(TIn value);
		public abstract TIn ConvertBack(TOut value);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Convert(default);

			if (!(value is TIn typedValue))
				throw new InvalidCastException();

			return Convert(typedValue);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return ConvertBack(default);

			if (!(value is TOut typedValue))
				throw new InvalidCastException();

			return ConvertBack(typedValue);
		}
	}
}
