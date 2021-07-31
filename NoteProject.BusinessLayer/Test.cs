using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            Noteproject.DataAccessLayer.EF.DatabaseContext db = new Noteproject.DataAccessLayer.EF.DatabaseContext();
            db.Categories.ToList();
        }
    }
}


/*
 
 
 
 */