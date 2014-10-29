using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Infrastructure.ModelMetadata.Filters
{
    public class ForiegnKeyByNameFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> _ForiegnKeyFieldNames =
            new HashSet<string>(StringComparer.Ordinal) { "Id" };

        public static IEnumerable<string> ForiegnKeyFieldNames
        {
            get { return _ForiegnKeyFieldNames; }
            set
            {
                _ForiegnKeyFieldNames.Clear();
                foreach (var item in value)
                {
                    _ForiegnKeyFieldNames.Add(item);
                }
            }
        }

        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.DataTypeName) &&
                ForiegnKeyFieldNames.Any(o => o != metadata.PropertyName && metadata.PropertyName.EndsWith(o)))
            {
                metadata.DataTypeName = metadata.PropertyName;
            }
        }
    }
}