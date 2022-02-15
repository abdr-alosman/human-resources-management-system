using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewKaratIk.Models;
using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity
            modelBuilder.Entity<User>().Property(b => b.NameSurname).HasComputedColumnSql("[Name]+' '+[Surname]");


            base.OnModelCreating(modelBuilder);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Pozisyon>? Pozisyons { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<Nitelik>? Niteliks { get; set; }
       
        public DbSet<interview> Interviews { get; set; }
        public DbSet<Aday> Adays { get; set; }
        public DbSet<InterviewUser> InterviewUsers { get; set; }
        public DbSet<MulakatDegerlendirme> MulakatDegerlendirmes { get; set; }
  
        public DbSet<Ozluk> Ozluks { get; set; }
        public DbSet<TeklifFormu> TeklifFormus { get; set; }
        public DbSet<AdayOnaylama> AdayOnaylamas { get; set; }
    }
}