using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CAS.Models
{
    public class RegisterModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage ="Yêu cầu nhập tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password",ErrorMessage ="Xác nhận mật khẩu không đúng")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Độ dài mật khẩu phải lớn hơn 6 ký tự")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu")]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Họ tên")]
        public string Name { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string Phone { set; get; }
    }
}