using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET02_WebApp.Models
{
    // 게시판을 위한 테이블 매핑 정보
    public class Board
    {
        // 테이블 PK 
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "아이디를 입력해주세요")] // Not Null
        [DisplayName("아이디")]
        public string UserId { get; set; }
        [DisplayName("이름")]
        public string? UserName { get; set; } // 뒤에 ? 를 붙이면 Null을 허용
        [Required(ErrorMessage = "제목을 입력해주세요")] // Not Null
        [MaxLength(200)] // Varchar(200)
        [DisplayName("제목")]
        public string Title { get; set; }
        [DisplayName("조회")]
        public int ReadCount { get; set; }
        [DisplayName("작성일")]
        public DateTime PostDate { get; set; }
        [DisplayName("내용")]
        public string Contents { get; set; }

        [DisplayName("번호")]
        [NotMapped]
        public int rowNum { get; set; }
    }
}
