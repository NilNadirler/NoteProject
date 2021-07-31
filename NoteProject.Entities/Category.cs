using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    [Table("Categories")]
    public class Category:MyEntityBase
    {
        [Required,StringLength(60)]
        public string Title { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; } //1 kategorinin birden fazla notu olabilir.

        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
