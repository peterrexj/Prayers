using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Models
{
    public class SinglePageDataModel
    {
        public int PageId { get; set; }
        public int? PreviousPageId { get; set; }
        public int? NextPageId { get; set; }
        public bool IsCurrentPage { get; set; }
        public List<PageContentModel> Content { get; set; }
    }
}
