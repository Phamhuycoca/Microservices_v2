using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entites
{
    public partial class refresh_token
    {
        /// <summary>
        /// Khóa chính của refresh token
        /// Mỗi refresh token là duy nhất
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Giá trị refresh token (random string)
        /// Dùng để xác thực khi client yêu cầu cấp lại access token
        /// 👉 KHÔNG nên là JWT
        /// 👉 Có thể lưu dạng hash để tăng bảo mật
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Khóa ngoại liên kết tới bảng người dùng (nguoi_dung)
        /// Xác định refresh token này thuộc về user nào
        /// </summary>
        public Guid nguoi_dung_id { get; set; }

        /// <summary>
        /// Thời điểm refresh token hết hạn
        /// Sau thời điểm này token không còn giá trị sử dụng
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Trạng thái thu hồi của refresh token
        /// true  = đã bị thu hồi (logout, rotate token, bị hack)
        /// false = còn hiệu lực
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// Thời điểm refresh token được tạo
        /// Dùng cho audit, debug, bảo mật
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Địa chỉ IP của client tại thời điểm tạo refresh token
        /// Dùng để phát hiện đăng nhập bất thường
        /// </summary>
        public string CreatedByIp { get; set; }
        public nguoi_dung nguoi_dung { get; set; }
    }
}
