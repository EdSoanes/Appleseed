using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace Appleseed.Code.Models
{
    public class BaseViewModel
    {
        public bool IsAjaxRequest { get; set; }

        public IPublishedContent CurrentPage
        {
            get;
            private set;
        }

        public BaseViewModel(IPublishedContent currentPage)
        {
            CurrentPage = currentPage;
        }
    }
}