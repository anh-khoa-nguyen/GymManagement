using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Manage
{
    public class ResultDTO
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        // Constructor riêng tư để buộc người dùng sử dụng các phương thức factory
        private ResultDTO(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Tạo một đối tượng kết quả thành công.
        /// </summary>
        public static ResultDTO Success()
        {
            return new ResultDTO(true);
        }

        /// <summary>
        /// Tạo một đối tượng kết quả thất bại với một thông điệp lỗi cụ thể.
        /// </summary>
        /// <param name="errorMessage">Thông điệp lỗi.</param>
        public static ResultDTO Failure(string errorMessage)
        {
            return new ResultDTO(false, errorMessage);
        }
    }
}
