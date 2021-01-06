using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SurveySystem.WebApi.Common
{
    public static class ReflectionHelper
    {
        public static string AttributeReader<TAttr>(IEnumerable<CustomAttributeData> attributes)
        {
            return attributes.FirstOrDefault(a => a.AttributeType == typeof(TAttr))?.ConstructorArguments.FirstOrDefault().Value?.ToString();
        }
    }
}

