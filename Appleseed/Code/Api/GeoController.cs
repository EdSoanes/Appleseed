using Appleseed.Code.Initialization;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using umbraco.cms.businesslogic.datatype;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Appleseed.Code.Api
{
    public class GeoController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;

        public GeoController()
        {
            _contentService = ApplicationStartup.Container.GetInstance<IContentService>();
            _contentTypeService = ApplicationStartup.Container.GetInstance<IContentTypeService>();
        }

        [HttpGet]
        public FeatureCollection AllMarkers()
        {
            var res = new FeatureCollection();

            var start = _contentService.GetRootContent().Where(x => x.ContentType.Alias == "Start").FirstOrDefault();
            if (start != null)
            {
                var t = _contentTypeService.GetContentType("Article");
                var articles = _contentService.GetContentOfContentType(t.Id);

                var helper = new UmbracoHelper(UmbracoContext.Current);

                foreach (var article in articles)
                {
                    double latitude = 0.0;
                    double longitude = 0.0;

                    if (double.TryParse(article.GetValue<string>("latitude"), out latitude) && double.TryParse(article.GetValue<string>("longitude"), out longitude))
                    {
                        var pos = new Point(new GeographicPosition(latitude, longitude));

                        var props = new Dictionary<string, object>();
                        props.Add("title", article.GetValue<string>("title"));
                        props.Add("description", article.GetValue<string>("teaserText"));
                        props.Add("type", "Adventure");
                        props.Add("url", helper.NiceUrl(article.Id));
                        props.Add("icon", new
                            {
                                iconUrl = GetMarkerIcon(article.GetValue<int>("mapMarker")),
                                iconSize = new int[] {  50, 50 },
                                iconAnchor = new int[]  { 25, 25 },
                                popupAnchor = new int[]  { 0, -25 }
                            });

                        var feature = new Feature(pos, props, article.Id.ToString());
                        res.Features.Add(feature);
                    }
                }
            }

            return res;
        }

        private string GetMarkerIcon(int mapMarkerType)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var valueStr = helper.GetPreValueAsString(mapMarkerType);

            switch (valueStr)
            {
                case "Biohazard": return "/css/img/biohazard.png";
                case "Battle": return "/css/img/battle.png";
                case "Radiation": return "/css/img/raditation.png";
                case "Death": return "/css/img/skull-crossbones.png";
                default: return "/css/img/marker.png";
            }
        }
    }
}