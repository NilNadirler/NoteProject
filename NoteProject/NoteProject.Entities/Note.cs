using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    public class Note:MyEntityBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }

        public virtual Category Category { get; set; } //Her not bir category aittir.
        public virtual EvernoteUser Owner { get; set; } //Her not bir kullanıcı tarafından yazılmıştır..
        public virtual List<Comment> Comments { get; set; } // 1 not altında birden fazla not tutabilir.
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
 
 */


















