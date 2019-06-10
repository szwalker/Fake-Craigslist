using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CatAreaViewModel
    {

        public IEnumerable<Area> Areas { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
