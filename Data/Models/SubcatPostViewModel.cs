using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class SubcatPostViewModel
    {
        public Subcategory Sub { get; set; }
        public IEnumerable<Post> Posts{ get; set; }

    }
}
