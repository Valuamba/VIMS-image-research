using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.View.Converters
{
	internal class ObjectToTypeConverter : ValueConverter<object, Type>
	{
		public override Type Convert(object value)
		{
			return value?.GetType();
		}

		public override object ConvertBack(Type value)
		{
			throw new NotSupportedException();
		}
	}
}
