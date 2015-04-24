using Appleseed.Code.Controllers;
using log4net;
using StructureMap;
using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Cache;
using Umbraco.Web.Mvc;

namespace Appleseed.Code.Initialization
{
    public class ApplicationStartup : ApplicationEventHandler
    {
        internal static Container Container
        {
            get;
            set;
        }

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DefaultController));

            Container = new Container();
            Container.Configure(x => x.Scan(y =>
            {
                y.AssemblyContainingType<ApplicationStartup>();
                y.SingleImplementationsOfInterface();
            }));

            Container.Configure(x => x.Scan(y => y.AddAllTypesOf<IController>()));
            Container.Configure(x => x.Scan(y => y.AddAllTypesOf<IHttpController>()));

            Container.Configure(x => x.For<ILog>().Use(() => LogManager.GetLogger("AsynchronousLog4NetAppender")));
            Container.Configure(x => x.For<IContentService>().Use(() => ApplicationContext.Current.Services.ContentService));
            Container.Configure(x => x.For<IContentTypeService>().Use(() => ApplicationContext.Current.Services.ContentTypeService));
            Container.Configure(x => x.For<IMediaService>().Use(() => ApplicationContext.Current.Services.MediaService));
            FilteredControllerFactoriesResolver.Current.RemoveType<RenderControllerFactory>();
            FilteredControllerFactoriesResolver.Current.AddType<MvcControllerFactory>();

            //ContentService.Published += ContentService_Published;
            //ContentService.UnPublished += ContentService_UnPublished;
            //ContentService.Creating += ContentService_Creating;

            //MediaService.Saved += MediaService_Saved;
            //MediaService.Trashed += MediaService_Trashed;

            //PageCacheRefresher.CacheUpdated += PageCacheRefresher_CacheUpdated;
        }

        //void ContentService_Creating(IContentService sender, Umbraco.Core.Events.NewEventArgs<IContent> e)
        //{
        //}

        //void PageCacheRefresher_CacheUpdated(PageCacheRefresher sender, Umbraco.Core.Cache.CacheRefresherEventArgs e)
        //{
        //}

        //void MediaService_Trashed(IMediaService sender, Umbraco.Core.Events.MoveEventArgs<IMedia> e)
        //{
        //}

        //void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        //{
        //    _contentIndexingService.Index(e.PublishedEntities);
        //}

        //void ContentService_UnPublished(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        //{
        //}

        //void MediaService_Saved(IMediaService sender, Umbraco.Core.Events.SaveEventArgs<IMedia> e)
        //{
        //}
    }

    public class MvcControllerFactory : RenderControllerFactory
    {
        public override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            return base.GetControllerType(requestContext, controllerName);
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var type = base.GetControllerType(requestContext, controllerName);

            return ApplicationStartup.Container.GetInstance(type) as Controller;
        }
    }
}