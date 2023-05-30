using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNET02_WebApp.Models
{
    public class CreateRoleModel
    {
        [Required(ErrorMessage = "권한명은 필수입니다.")]
        [DisplayName("권한명")]
        public string RoleName { get; set; } // Admin, User

    }
}
