using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Infrastructure.ModelMetadata.Filters
{
    public class TextAreaByNameFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> _TextAreaFieldNames =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Body", "Comments", "Notes" };

        public static IEnumerable<string> TextAreaFieldNames
        {
            get { return _TextAreaFieldNames; }
            set
            {
                _TextAreaFieldNames.Clear();
                foreach (var item in value)
                {
                    _TextAreaFieldNames.Add(item);
                }
            }
        }

        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.DataTypeName) &&
                TextAreaFieldNames.Contains(metadata.PropertyName))
            {
                metadata.DataTypeName = "MultilineText";
            }
        }
    }
}