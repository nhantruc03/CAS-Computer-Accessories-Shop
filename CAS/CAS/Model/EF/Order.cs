namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        public DateTime? CreateDate { get; set; }

        public long CustomerID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên người nhận")]
        public string ShipName { get; set; }

        [StringLength(50)]
        [Display(Name = "Số điện thoại người nhận")]
        public string ShipMobile { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        [Display(Name = "Email người nhận")]
        public string ShipEmail { get; set; }

        public bool Status { get; set; }

        [StringLength(50)]
        [Display(Name = "ID mã giảm giá")]
        public string DiscountCodeID { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal? Total { get; set; }
    }
}
