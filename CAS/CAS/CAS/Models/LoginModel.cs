using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CAS.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="Phải nhập tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Mập khẩu")]
        [Required(ErrorMessage = "Phải nhập mật khẩu")]
        public string PassWord { get; set; }

    }
}