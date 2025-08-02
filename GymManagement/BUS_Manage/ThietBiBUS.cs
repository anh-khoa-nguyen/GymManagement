using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using BUS_Manage.Helpers;
using DAL_Manage;
using DAL_Manage.Models;
using DTO_Manage;

namespace BUS_Manage
{
    public class ThietBiBUS
    {
        private readonly ThietBiDAL thietBiDAL = new ThietBiDAL();
        private readonly IMapper _mapper; // Khai báo đối tượng mapper
        public ThietBiBUS()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }

        public List<ThietBiDTO> GetAll()
        {
            var thietBiListFromDb = thietBiDAL.GetAll();
            // Sử dụng instance mapper đã được khởi tạo
            return _mapper.Map<List<ThietBiDTO>>(thietBiListFromDb);
        }

        public ResultDTO Add(ThietBiDTO dto)
        {
            // Sử dụng instance mapper đã được khởi tạo
            var thietBi = _mapper.Map<ThietBi>(dto);
            if (thietBiDAL.Add(thietBi))
            {
                return ResultDTO.Success();
            }
            return ResultDTO.Failure("Thêm thiết bị thất bại.");
        }

        public ResultDTO Update(ThietBiDTO dto)
        {
            var thietBi = _mapper.Map<ThietBi>(dto);
            if (thietBiDAL.Update(thietBi))
            {
                return ResultDTO.Success();
            }
            return ResultDTO.Failure("Cập nhật thiết bị thất bại.");
        }

        public ResultDTO Delete(int id)
        {
            if (thietBiDAL.Delete(id))
            {
                return ResultDTO.Success();
            }
            return ResultDTO.Failure("Xóa thiết bị thất bại.");
        }

        public object GetTinhTrangDataSource()
        {
            // Lấy tất cả các giá trị của enum TinhTrangThietBi
            return Enum.GetValues(typeof(DTO_Manage.TinhTrangThietBi))
                       .Cast<DTO_Manage.TinhTrangThietBi>()
                       .Select(e => new { Value = e, Description = EnumHelper.GetDescription(e) })
                       .ToList();
        }
    }
}