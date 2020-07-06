namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên tin tức")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên đường dẫn để hiển thị")]
        public string Metatitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Descriptions { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        [Display(Name = "Loại tin tức")]
        public string Tags { get; set; }
    }
}
