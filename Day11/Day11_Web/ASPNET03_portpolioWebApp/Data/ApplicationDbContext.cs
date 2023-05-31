using ASPNET02_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ASPNET03_portfolioWebApp.Models;

namespace ASPNET02_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext // 1. ASP.NET Identity : DbContext -> IdentityDbContext 결국 DbContext 쓰는것하고 동일
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        // 보드라는 테이블을 만들기 위한 컬럼 수는 1개
        public DbSet<Board> Boards { get; set; }
        // 포트폴리오 DB로 관리하기 위한 모델  
        public DbSet<PortfolioModel> Portfolios { get; set; }
        // 여기 우리가 안 만들었는데 하나가 생성됌
    }
}
