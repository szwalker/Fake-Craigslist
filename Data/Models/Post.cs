using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }//our job

        public DateTime Expiration { get; set; }//our job

        public ApplicationUser Owner { get; set; }//our job

        [Required]
        [Display(Name = "Title")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [Display(Name ="Body")]
        public string Body { get; set; }

        public Boolean Viewable { get; set; }//our job


        [Required]
        public Area PostArea { get; set; }

        [Required]
        public Locale PostLocale { get; set; }

        [Required]
        public Category PostCategory { get; set; }

        [Required]
        public Subcategory PostSubcategory { get; set; }

    }
}
