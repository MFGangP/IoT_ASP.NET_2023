using ASPNET02_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET02_WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        // 보드라는 테이블을 만들기 위한 컬럼 수는 1개
        public DbSet<Board> Boards { get; set; }
    }
}
