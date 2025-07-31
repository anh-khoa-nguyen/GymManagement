using DAL_Manage.DAL;
using DAL_Manage.Models;
using DTO_Manage;     
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_Manage
{
    public class LoaiGoiTapBUS
    {
        private LoaiGoiTapDAL loaiGoiTapDAL = new LoaiGoiTapDAL();

        // Hàm lấy danh sách DTO để hiển thị cho GUI
        public List<LoaiGoiTapDTO> GetAll()
        {
            // Gọi DAL để lấy danh sách Entity
            List<LoaiGoiTap> listEntity = loaiGoiTapDAL.GetAll();

            // Chuyển đổi từ List<Entity> sang List<DTO>
            List<LoaiGoiTapDTO> listDTO = listEntity.Select(e => new LoaiGoiTapDTO
            {
                ID = e.ID,
                Ten = e.Ten
            }).ToList();

            return listDTO;
        }

        // Hàm thêm, nhận vào DTO từ GUI
        public void Add(LoaiGoiTapDTO dto)
        {
            // Chuyển đổi từ DTO sang Entity để lưu xuống CSDL
            LoaiGoiTap entity = new LoaiGoiTap
            {
                Ten = dto.Ten
            };
            loaiGoiTapDAL.Add(entity);
        }

        // Hàm sửa, nhận vào DTO từ GUI
        public void Update(LoaiGoiTapDTO dto)
        {
            LoaiGoiTap entity = new LoaiGoiTap
            {
                ID = dto.ID,
                Ten = dto.Ten
            };
            loaiGoiTapDAL.Update(entity);
        }

        // Hàm xóa, chỉ cần ID
        public void Delete(int id)
        {
            loaiGoiTapDAL.Delete(id);
        }
    }
}
