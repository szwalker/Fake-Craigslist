using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data.Models
{
    public class HomePageViewModel
    {
        public virtual IEnumerable<Category> Cat { get; set; }
        public virtual IEnumerable<Subcategory> SubCat { get; set; }
        public IEnumerable<Area> Area { get; set; }
        public IEnumerable<Locale> Locale { get; set; }
    }
}
