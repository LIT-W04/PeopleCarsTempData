using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PeopleCarsTempData.Data;

namespace PeopleCarsTempData.Web.Models
{
    public class IndexViewModel
    {
        public IEnumerable<PersonWithCarCount> People { get; set; }
        public string GreenMessage { get; set; }
        public string RedMessage { get; set; }
    }
}