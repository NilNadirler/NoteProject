using NoteProject.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.BusinessLayer
{
    public class Repository<T> where T:class //Repository parametre olarak sadece class gönderebilir.
    {
        private Noteproject.DataAccessLayer.EF.DatabaseContext db = new Noteproject.DataAccessLayer.EF.DatabaseContext();
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = db.Set<T>();
        }
        public List<T> List()
        {
            return _objectSet.ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            return Save();
        }
        public int Update(T obj)
        {
            return Save();
        }
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
