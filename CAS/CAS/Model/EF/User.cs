﻿namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tài khoản")]
        //[Required(ErrorMessage = "Mời nhập User Name!")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Display(Name = "Mật khẩu")]
        //[Required(ErrorMessage = "Mời nhập Mật khẩu!")]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Họ tên")]
        //[Required(ErrorMessage = "Mời nhập Họ tên!")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        //[Required(ErrorMessage = "Mời nhập Địa chỉ!")]
        public string Address { get; set; }

        [StringLength(50)]
        //[Required(ErrorMessage = "Mời nhập Email!")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        //[Required(ErrorMessage = "Mời nhập Số điện thoại!")]
        public string Phone { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [StringLength(20)]
        [Display(Name = "Nhóm người dùng")]
        public string GroupID { get; set; }
    }
}
