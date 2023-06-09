﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNET02_WebApp.Models
{
    public class EditRoleModel
    {
        [DisplayName("권한아이디")]
        public string Id { get; set; }
        [DisplayName("권한이름")]
        [Required(ErrorMessage = ("권한이름이 필요합니다."))]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

        public EditRoleModel()
        {
            Users = new List<string>();
        }
    }
}
