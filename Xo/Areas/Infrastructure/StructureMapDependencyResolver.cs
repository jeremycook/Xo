using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xo.Areas.Infrastructure
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly Func<IContainer> ContainerFactory;

        public StructureMapDependencyResolver(Func<IContainer> containerFactory)
        {
            ContainerFactory = containerFactory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            return serviceType.IsAbstract || serviceType.IsInterface ?
                ContainerFactory().TryGetInstance(serviceType) :
                ContainerFactory().GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ContainerFactory().GetAllInstances(serviceType).Cast<object>();
        }
    }
}