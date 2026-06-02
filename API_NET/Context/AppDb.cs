using API_NET.Models;
using Microsoft.EntityFrameworkCore;

namespace API_NET.Context
{
    public class AppDb:DbContext
    {
        // ctor snippet para crear el constructor
        public AppDb(DbContextOptions<AppDb> options): base(options){}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().ToTable("users"); Cambiar el nombre de la tabla para la bbdd
            modelBuilder.Entity<User>(table =>
            {
                //table.HasKey("UserId"); || table.HasKey(row => row.Id); PRIMARY KEY
                //table.Property(row => row.Id).ValueGeneratedOnAdd(); AUTO INCREMENT
                table.Property(row => row.Name).IsRequired().HasMaxLength(50);
                table.Property(row => row.Email).IsRequired().HasMaxLength(50);
                table.Property(row => row.Password).IsRequired().HasMaxLength(50);
            });
        }
    }
}
