using Appleseed.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Appleseed.Code.Controllers
{
    public class DefaultController : RenderMvcController
    {
        public DefaultController()
        {
        }

        public override ActionResult Index(RenderModel model)
        {
            var res = new BaseViewModel(model.Content);
            res.IsAjaxRequest = this.Request.IsAjaxRequest();
            return CurrentTemplate(res);
        }
    }
}