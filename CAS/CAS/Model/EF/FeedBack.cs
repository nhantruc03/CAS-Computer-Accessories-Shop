namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBack")]
    public partial class FeedBack
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mời nhập Tên!")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mời nhập Số điện thoại!")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mời nhập Email!")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mời nhập Địa chỉ!")]
        public string Address { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Mời nhập Nội dung!")]
        public string Content { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool Status { get; set; }
    }
}
