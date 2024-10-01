using AiEntegrasyonProjesi.Models;
using Microsoft.EntityFrameworkCore;

namespace AiEntegrasyonProjesi.Data
{
    public class DbBaglanti:DbContext
    {
        public DbBaglanti(DbContextOptions<DbBaglanti> options) : base(options) { }

        public DbSet<AiResponse> AiResponse { get; set; }
    }
}
