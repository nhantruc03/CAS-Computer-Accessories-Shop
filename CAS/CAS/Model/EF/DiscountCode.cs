namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiscountCode")]
    public partial class DiscountCode
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên mã giả giảm")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Mô tả")]
        public string Descriptions { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Phần trăm giả giá")]
        public int? Percent { get; set; }

        [Display(Name = "Giá trị giảm tối đa (nếu trống sẽ giảm trên tổng hóa đơn)")]
        public long? MaxValue { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
