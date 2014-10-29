using System;
using System.Collections.Generic;

namespace Xo.Areas.Infrastructure.ModelMetadata.Filters
{
    public class EnumDataTypeFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                (metadata.ModelType.IsEnum || (metadata.IsNullableValueType && metadata.ModelType.GetGenericArguments()[0].IsEnum)))
            {
                if (string.IsNullOrEmpty(metadata.TemplateHint))
                {
                    metadata.TemplateHint = metadata.IsNullableValueType ?
                        metadata.ModelType.GetGenericArguments()[0].Name :
                        metadata.ModelType.Name;
                }

                if (string.IsNullOrEmpty(metadata.DataTypeName))
                {
                    metadata.DataTypeName = "Enum";
                }
            }
        }
    }
}
