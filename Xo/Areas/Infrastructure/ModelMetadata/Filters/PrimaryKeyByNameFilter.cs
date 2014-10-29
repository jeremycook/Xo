using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Infrastructure.ModelMetadata.Filters
{
    public class PrimaryKeyByNameFilter : IModelMetadataFilter
    {
        private static readonly HashSet<string> _PrimaryKeyFieldNames =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Id" };

        public static IEnumerable<string> PrimaryKeyFieldNames
        {
            get { return _PrimaryKeyFieldNames; }
            set
            {
                _PrimaryKeyFieldNames.Clear();
                foreach (var item in value)
                {
                    _PrimaryKeyFieldNames.Add(item);
                }
            }
        }

        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.TemplateHint) &&
                PrimaryKeyFieldNames.Contains(metadata.PropertyName))
            {
                metadata.TemplateHint = "HiddenInput";
            }
        }
    }
}