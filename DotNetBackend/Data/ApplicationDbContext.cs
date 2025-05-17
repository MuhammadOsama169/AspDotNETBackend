using Microsoft.EntityFrameworkCore;
using DotNetBackend.Models;
using System.Collections.Generic;

namespace DotNetBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OtpEntry> Otps { get; set; } 
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        //public DbSet<Workspace> Workspaces { get; set; }
    }
}
