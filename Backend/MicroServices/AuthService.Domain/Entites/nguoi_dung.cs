using AuthService.Domain.BaseEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entites
{
    public class nguoi_dung : IdentityUser<Guid>
    {
        public string? ho_ten { get; set; }
        public string? ten_dem { get; set; }
        public string? ten_day_du { get; set; }
        public DateTime? ngay_sinh { get; set; }
        public DateTime? ngay_tao { get; set; }
        public Guid? nguoi_tao_id { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public virtual nguoi_dung? nguoi_tao { get; set; }
        public virtual nguoi_dung? nguoi_chinh_sua { get; set; }
        public virtual ICollection<refresh_token>? Refresh_Tokens { get; set; }
    }
}
