using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace Appleseed.Code.Models
{
    public class StartViewModel : BaseViewModel
    {
        public IEnumerable<IPublishedContent> Menu { get; set; }

        public StartViewModel(IPublishedContent currentPage)
            : base(currentPage)
        {
        }
    }
}