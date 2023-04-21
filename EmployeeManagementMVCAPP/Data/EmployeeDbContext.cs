using EmployeeManagementMVCAPP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementMVCAPP.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<EmployeeMaster> EmployeeMasters { get; set; }
    }
}
