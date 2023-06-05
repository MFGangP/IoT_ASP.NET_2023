using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TodoItemAPP.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        [Column("Title", TypeName = "Varchar(100)")]
        public string? Title { get; set; }
        public string ToDate { get; set; }
        public int IsComplete { get; set; }

    }
}
