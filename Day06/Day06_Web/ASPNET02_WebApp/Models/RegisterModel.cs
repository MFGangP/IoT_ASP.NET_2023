﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNET02_WebApp.Models
{
    public class RegisterModel
    {
        // 회원가입 할 때 데이터 받는 부분
        [Required(ErrorMessage = "이메일 주소는 필수입니다.")]
        [EmailAddress]
        [DisplayName("이메일 주소")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("휴대폰번호")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "패스워드는 필수입니다.")]
        [DataType(DataType.Password)]
        [DisplayName("패스워드")]
        public string Password { get; set; }

        [Required(ErrorMessage = "패스워드는 필수입니다.")]
        [DataType(DataType.Password)]
        [DisplayName("패스워드 확인")]
        [Compare("Password", ErrorMessage = "패스워드가 일치하지 않습니다. 다시 입력하세요.")]
        public string ConfirmPassword { get; set; }

    }
}
