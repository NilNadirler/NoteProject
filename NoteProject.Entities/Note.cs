using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    
    public class Note:MyEntityBase
    {
        [Required,StringLength(600)]
        public string Title { get; set; }
        [Required, StringLength(2000)]
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int CategoryId { get; set; } 

        public virtual Category Category { get; set; } //Her not bir category aittir.
        public virtual EvernoteUser Owner { get; set; } //Her not bir kullanıcı tarafından yazılmıştır..
        public virtual List<Comment> Comments { get; set; } // 1 not altında birden fazla not tutabilir.

        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}

/* 
 * 
 * 
 * 
 * 
 * CATEGORY
   Yazılım 
        -C#(Emre Owner)
            --Bu ne biçim not(Hikmet)
            --Bence olmuş yaa(Özet)

        -Java(Ömer Owner)
            --VAvvv(Hikmet)
            --Saçma(Özet)
 

  public int CategoryId { get; set; } :
        * Database'den note çektiğim zaman ilk 4 property getirecek ve ben category Id'sini istediğimiz zaman bir select sorgusu daha yazacağız. Ama ben CategoryId property'sini eklediğimde için ilk select ile bana bu bilgiyi getirecek.
 */


















