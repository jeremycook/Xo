using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xo.Infrastructure
{
    public class StructureMapFilterProvider : FilterAttributeFilterProvider
    {
        private readonly Func<IContainer> ContainerFactory;

        public StructureMapFilterProvider(Func<IContainer> containerFactory)
        {
            ContainerFactory = containerFactory;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var container = ContainerFactory();
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            foreach (var filter in filters)
            {
                container.BuildUp(filter.Instance);
                yield return filter;
            }
        }
    }
}