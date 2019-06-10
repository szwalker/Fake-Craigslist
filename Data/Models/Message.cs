using Data.Models;
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
    public class Message
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,Display(Name ="Subject"),MaxLength(50),MinLength(1)]
        public string Subject { get; set; }

        [Required,Display(Name ="Message body")]
        public string Content { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public int PostId { get; set; }
    }
}
