using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    [Table("Likes")]
    public class Liked  
    {
        public int Id { get; set; }
        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }
    }
}
