using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Models
{
    public class PageContentModel
    {
        public int Id { get; set; }
        public double FontSize { get; set; }
        public string[] FontAttribute { get; set; }
        public string TextWrap { get; set; }
        public string FontAlign { get; set; }
        public string ContentType { get; set; }
        public int? SpaceBefore { get; set; }
        public string AdditionalData { get; set; }
        public bool IsHeader {  get; set; }
        public string Content { get; set; }
    }
}
