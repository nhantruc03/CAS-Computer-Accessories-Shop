namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Document")]
    public partial class Document
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên tài liệu")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết tài liệu")]
        public string Detail { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
