using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    //test
    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [Display(Name = "Area Name")]
        public string Name { get; set; }

        public virtual ICollection<Locale> Locales { get; set; }

        public Boolean Viewable { get; set; }


    }
}
