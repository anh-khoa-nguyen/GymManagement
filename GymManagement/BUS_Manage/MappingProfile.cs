using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL_Manage.Models;
using DTO_Manage;
using BUS_Manage.Helpers;


namespace BUS_Manage
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Định nghĩa quy tắc ánh xạ từ Model -> DTO
            CreateMap<ThietBi, ThietBiDTO>()
            .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.Phong.Ten))
            .ForMember(dest => dest.TinhTrangHienThi,
                           opt => opt.MapFrom(src => EnumHelper.GetDescription(src.Tinh_Trang)));

            // Định nghĩa quy tắc ánh xạ từ DTO -> Model
            CreateMap<ThietBiDTO, ThietBi>();
        }
    }
}
