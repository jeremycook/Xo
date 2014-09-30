using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Infrastructure.ModelMetadata.Filters
{
    public class LabelConventionFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DisplayName))
            {
                metadata.DisplayName = metadata.PropertyName.Humanize(LetterCasing.Sentence);
            }
        }
    }
}