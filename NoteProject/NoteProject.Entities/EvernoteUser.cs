using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    public class EvernoteUser:MyEntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } //noteproject/user/activate/asbd-asda-wqeq-sdaa
        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; } //1 kullanıcı birden fazla not yazabilir.
        public virtual List<Comment> Comments { get; set; } //1 kullanıcı birden fazla yorum yazabilir.
    }
}
