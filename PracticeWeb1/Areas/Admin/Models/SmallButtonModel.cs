using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Areas.Admin.Models
{
    public class SmallButtonModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Type { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }
        public string ActionParameters {
            get {
                return (Id != 0) ? string.Format("?Id={0}", Id) : null;
            } }
    }
}