using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Entities
{
    public class MyEntityBase
    {
        //[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)] Yazmak zorunda değiliz zaten EF tarafından Id isimli her property bu özellikleri default olarak alır. 
        public int Id { get; set; }

        
        public DateTime CreateOn { get; set; }
        
        public DateTime ModifiedOn { get; set; }
        
        public string ModifiedUsername { get; set; }
    }
}
