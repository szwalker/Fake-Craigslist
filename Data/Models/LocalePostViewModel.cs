using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class LocalePostViewModel
    {
        public Locale Loc { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
