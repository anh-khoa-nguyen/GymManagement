using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_Manage.Models;

namespace DAL_Manage.DAL
{
    public class PhongDAL
    {
        public List<Phong> GetAll()
        {
            using (var db = new GymModel())
            {
                return db.Phong.ToList();
            }
        }

        public void Add(Phong phong)
        {
            using (var db = new GymModel())
            {
                db.Phong.Add(phong);
                db.SaveChanges();
            }
        }

        public void Update(Phong phong)
        {
            using (var db = new GymModel())
            {
                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new GymModel())
            {
                var phong = db.Phong.Find(id);
                if (phong != null)
                {
                    db.Phong.Remove(phong);
                    db.SaveChanges();
                }
            }
        }
    }
}