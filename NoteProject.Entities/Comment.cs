using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        [Required,StringLength(300)]
        public string Text { get; set; }
        public Note Note { get; set; } //her yorum 1 not aittir.
        public EvernoteUser Owner { get; set; } //her yorumu 1 kullanıcı yazmıştır.
    }
}
