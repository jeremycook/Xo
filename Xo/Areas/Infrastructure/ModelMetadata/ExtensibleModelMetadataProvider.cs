using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Xo.Areas.Infrastructure.ModelMetadata
{
    public class ExtensibleModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private readonly IModelMetadataFilter[] MetadataFilters;

        public ExtensibleModelMetadataProvider(IModelMetadataFilter[] metadataFilters)
        {
            MetadataFilters = metadataFilters;
        }

        protected override System.Web.Mvc.ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            foreach (var filter in MetadataFilters)
            {
                filter.TransformMetadata(metadata, attributes);
            }

            return metadata;
        }
    }
}