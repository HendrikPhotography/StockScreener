using Microsoft.EntityFrameworkCore;

namespace StockScreener.Data
{
    public class StockScreenerDbContext : DbContext
    {
        public StockScreenerDbContext(DbContextOptions<StockScreenerDbContext> options)
            : base(options) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
    }
}