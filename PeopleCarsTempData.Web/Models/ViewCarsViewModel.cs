using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PeopleCarsTempData.Data;

namespace PeopleCarsTempData.Web.Models
{
    public class ViewCarsViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public Person Person { get; set; }
    }
}