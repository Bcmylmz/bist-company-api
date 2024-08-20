using AllCompaniesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AllCompaniesAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }



        public DbSet<Company> Companies { get; set; }
    }
}
