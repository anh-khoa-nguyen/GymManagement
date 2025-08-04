using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Helpers
{
    public static class RoleHelper
    {
        public static string GetVietnameseRoleName(string roleName)
        {
            switch (roleName)
            {
                case "QuanLy":
                    return "Quản Lý";
                case "PT":
                    return "Huấn Luyện Viên (PT)";
                case "HoiVien":
                    return "Hội Viên";
                default:
                    return roleName; 
            }
        }
    }
}