using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser:MyEntityBase
    {
        [StringLength(25),DisplayName("İsim")]
        public string Name { get; set; }
        [StringLength(25), DisplayName("Soyisim")]
        public string Surname { get; set; }
        [Required,StringLength(25), DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }
        [Required, StringLength(70), DisplayName("E-Mail")]
        public string Email { get; set; }
        [Required, StringLength(25), DisplayName("Şifre")]
        public string Password { get; set; }

        [StringLength(30)] //user12.png
        public string ProfileImageFilename { get; set; }

        public bool IsActive { get; set; } //noteproject/user/activate/asbd-asda-wqeq-sdaa
        public bool IsAdmin { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }
        

        public virtual List<Note> Notes { get; set; } //1 kullanıcı birden fazla not yazabilir.
        public virtual List<Comment> Comments { get; set; } //1 kullanıcı birden fazla yorum yazabilir.
        public virtual List<Liked> Likes { get; set; }

        public EvernoteUser()
        {
            Notes = new List<Note>();
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
