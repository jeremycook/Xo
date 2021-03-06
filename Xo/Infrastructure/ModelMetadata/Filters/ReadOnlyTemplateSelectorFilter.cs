﻿using System;
using System.Collections.Generic;

namespace Xo.Infrastructure.ModelMetadata.Filters
{
    public class ReadOnlyTemplateSelectorFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (metadata.IsReadOnly)
            {
                // The template specified by TemplateHint takes precedence over
                // DataTypeName and will be used if a template by the name is present.
                if (string.IsNullOrEmpty(metadata.TemplateHint))
                {
                    metadata.TemplateHint = "ReadOnly" + metadata.ModelType.Name;
                }

                // Fallback to a template named "ReadOnly" if DataTypeName is
                // not already set.
                if (string.IsNullOrEmpty(metadata.DataTypeName))
                {
                    metadata.DataTypeName = "ReadOnly";
                }
            }
        }
    }
}
