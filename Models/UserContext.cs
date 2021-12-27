using Microsoft.EntityFrameworkCore;

namespace EFCoreTest.Models
{
    public class UserContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        /// <summary>
        /// 订单
        /// </summary>
        public DbSet<Order> Order { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Role { get; set; }
        /// <summary>
        /// 简历
        /// </summary>
        public DbSet<Resume> Resume { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(x => x.Resume).WithOne(x => x.User).HasForeignKey<Resume>(x => x.UserID);//一对一
                entity.HasMany(x => x.Orders).WithOne(x => x.User);//一对多
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(b => new { b.RoleId, b.UserId });//设置联合主键
                entity.HasOne(b => b.User).WithMany(b => b.UserRoles);
                entity.HasOne(b => b.Role).WithMany(b => b.UserRoles);
            });
            base.OnModelCreating(modelBuilder);
        }

        public static readonly ILoggerFactory loggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
            });


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
        }
    }
}
