using DAL_Manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL_Manage
{
    public class ThietBiDAL
    {
        public List<ThietBi> GetAll()
        {
            using (var db = new GymModel())
            {
                return db.ThietBi.Include(tb => tb.Phong).ToList();
            }
        }

        public bool Add(ThietBi thietBi)
        {
            using (var db = new GymModel())
            {
                try
                {
                    db.ThietBi.Add(thietBi);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool Update(ThietBi thietBi)
        {
            using (var db = new GymModel())
            {
                try
                {
                    // Attach đối tượng vào context và đánh dấu là đã bị thay đổi
                    // Đây là cách cập nhật hiệu quả, không cần đọc dữ liệu cũ từ DB
                    db.ThietBi.Attach(thietBi);
                    db.Entry(thietBi).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var db = new GymModel())
            {
                try
                {
                    ThietBi thietBiToDelete = db.ThietBi.Find(id);
                    if (thietBiToDelete != null)
                    {
                        db.ThietBi.Remove(thietBiToDelete);
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}