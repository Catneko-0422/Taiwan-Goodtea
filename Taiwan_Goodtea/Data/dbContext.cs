using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TAIWAN_GoodTea.Data
{
    public class dbContext : IdentityDbContext
    {

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }
    }
}

    
