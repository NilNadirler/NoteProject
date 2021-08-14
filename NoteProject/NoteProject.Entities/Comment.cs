using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    public class Comment : MyEntityBase
    {
        public string Text { get; set; }

        public Note Note { get; set; } //her yorum 1 not aittir.
        public EvernoteUser Owner { get; set; } //her yorumu 1 kullanıcı yazmıştır.
    }
}
