using Appleseed.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;

namespace Appleseed.Code.Controllers
{
    public class StartController : RenderMvcController
    {
        public StartController()
        {
        }

        public override ActionResult Index(RenderModel model)
        {
            var res = new StartViewModel(model.Content);

            var info = model.Content.Children().Where(x => x.IsVisible() && x.Name == "Info").FirstOrDefault();
            var menuItems = info.Children().Where(x => x.IsVisible() && x.DocumentTypeAlias == "ArticleList");
            res.Menu = menuItems;

            return CurrentTemplate(res);
        }
    }
}