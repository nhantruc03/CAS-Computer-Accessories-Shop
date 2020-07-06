namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Đường dẫn để hiển thị")]
        public string Metatitle { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string Descriptions { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình ảnh chính")]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        [Display(Name = "Hình ảnh phụ")]
        public string MoreImages { get; set; }

        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        public decimal? PromotionPrice { get; set; }

        [Display(Name = "Bao gồm thuế VAT")]
        public bool IncludeVAT { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Danh mục sản phẩm")]
        public long? CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Bảo hành")]
        public int? Warranty { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        public int? ViewCount { get; set; }
    }
}
