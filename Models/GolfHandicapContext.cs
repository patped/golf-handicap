using Microsoft.EntityFrameworkCore;

namespace GolfHandicap.Models
{
    public class GolfHandicapContext : DbContext
    {
        public GolfHandicapContext(DbContextOptions<GolfHandicapContext> options)
            : base(options)
        {

        }

        public DbSet<PlayerHandicap> PlayerHandicaps { get; set; }
    }
}
