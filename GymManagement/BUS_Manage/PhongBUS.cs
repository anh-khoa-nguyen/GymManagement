using System;
using System.Collections.Generic;
using System.Linq;
using DAL_Manage.DAL;
using DAL_Manage.Models;
using DTO_Manage;

namespace BUS_Manage
{
    public class PhongBUS
    {
        private PhongDAL phongDAL = new PhongDAL();

        public List<PhongDTO> GetAll()
        {
            List<Phong> listEntity = phongDAL.GetAll();
            return listEntity.Select(e => new PhongDTO
            {
                ID = e.ID,
                Ten = e.Ten,
                Dia_Chi = e.Dia_Chi
            }).ToList();
        }

        public ResultDTO Add(PhongDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Ten))
            {
                return ResultDTO.Failure("Tên phòng không được để trống.");
            }

            Phong entity = new Phong
            {
                Ten = dto.Ten,
                Dia_Chi = dto.Dia_Chi
            };

            try
            {
                phongDAL.Add(entity);
                return ResultDTO.Success();
            }
            catch (Exception ex)
            {
                return ResultDTO.Failure("Lỗi khi thêm phòng: " + ex.Message);
            }
        }

        public ResultDTO Update(PhongDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Ten))
            {
                return ResultDTO.Failure("Tên phòng không được để trống.");
            }

            Phong entity = new Phong
            {
                ID = dto.ID,
                Ten = dto.Ten,
                Dia_Chi = dto.Dia_Chi
            };

            try
            {
                phongDAL.Update(entity);
                return ResultDTO.Success();
            }
            catch (Exception ex)
            {
                return ResultDTO.Failure("Lỗi khi cập nhật phòng: " + ex.Message);
            }
        }

        public ResultDTO Delete(int id)
        {
            try
            {
                phongDAL.Delete(id);
                return ResultDTO.Success();
            }
            catch (Exception ex)
            {
                // Bắt lỗi khóa ngoại khi xóa phòng đang có thiết bị hoặc nhân viên
                if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return ResultDTO.Failure("Không thể xóa phòng này vì đang có thiết bị hoặc nhân viên được gán.");
                }
                return ResultDTO.Failure("Lỗi khi xóa phòng: " + ex.Message);
            }
        }
    }
}