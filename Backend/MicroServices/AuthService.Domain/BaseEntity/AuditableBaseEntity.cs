using AuthService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.BaseEntity
{
    public partial class AuditableBaseEntity
    {
        public virtual Guid Id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public Guid? nguoi_tao_id { get; set; }
        public DateTime? ngay_chinh_sua { get; set; }
        public Guid? nguoi_chinh_sua_id { get; set; }
        public virtual nguoi_dung nguoi_tao { get; set; }
        public virtual nguoi_dung nguoi_chinh_sua { get; set; }
    }
}
