using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CAS.Models
{
    public class UserPassword
    {
        [Display(Name = "Mật khẩu cũ")]
        //[Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu phải lớn hơn 6 ký tự")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu mới")]
        public string NewPassword { set; get; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu phải lớn hơn 6 ký tự")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu mới")]
        public string ConfirmNewPassword { set; get; }


    }
}