using Microsoft.EntityFrameworkCore;

namespace TodoApiServer.Models
{
    public class ApplicationDbContext : DbContext
    {
        // 생성자 마법사로 만들것
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        // 이거 없으면 DB랑 연결이 안됌
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
