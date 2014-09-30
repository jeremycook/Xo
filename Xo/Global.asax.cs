using StructureMap;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Xo.Infrastructure;
using Xo.Infrastructure.ModelMetadata;
using Xo.Infrastructure.Tasks;

namespace Xo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static StructureMap.Container _AppContainer;

        /// <summary>
        /// Returns the application container once lazily created.
        /// </summary>
        public IContainer AppContainer
        {
            get
            {
                if (_AppContainer == null)
                {
                    // Create before configuring so that configuration can safely
                    // refer to the application container.
                    _AppContainer = new Container();
                    _AppContainer.Configure(cfg =>
                    {
                        cfg.AddRegistry(new StandardRegistry());
                        cfg.AddRegistry(new ControllerRegistry());
                        cfg.AddRegistry(new ActionFilterRegistry(() => Container ?? _AppContainer));
                        cfg.AddRegistry(new MvcRegistry());
                        cfg.AddRegistry(new TaskRegistry());
                        cfg.AddRegistry(new ModelMetadataRegistry());
                    });
                }

                return _AppContainer;
            }
        }

        /// <summary>
        /// Per HTTP request nested container. Setup and tore down by <see cref="Application_BeginRequest"/>
        /// and <see cref="Application_EndRequest"/>, respectively.
        /// </summary>
        public IContainer Container
        {
            get { return (IContainer)HttpContext.Current.Items["MvcApplication:Container"]; }
            set { HttpContext.Current.Items["MvcApplication:Container"] = value; }
        }

        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(() => Container ?? AppContainer));

            // Run init and startup tasks.
            using (var container = AppContainer.GetNestedContainer())
            {
                foreach (var task in container.GetAllInstances<IRunAtInit>())
                {
                    task.Execute();
                }

                foreach (var task in container.GetAllInstances<IRunAtStartup>())
                {
                    task.Execute();
                }
            }
        }

        public void Application_BeginRequest()
        {
            // Create a nested container that allows there to be one DbContext (and other things)
            // to be shared for the duration of each HTTP request.
            Container = AppContainer.GetNestedContainer();

            foreach (var task in Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }

        public void Application_Error()
        {
            foreach (var task in Container.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }

        public void Application_EndRequest()
        {
            try
            {
                foreach (var task in Container.GetAllInstances<IRunAfterEachRequest>())
                {
                    task.Execute();
                }
            }
            finally
            {
                Container.Dispose();
                Container = null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            AppContainer.Dispose();
            Container = null;
        }
    }
}
