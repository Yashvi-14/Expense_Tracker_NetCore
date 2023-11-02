using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Proxies;
namespace Expense_Tracker.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
      /*  public virtual DbSet<CountryMaster> CountryMaster { get; set; }*/ 
        public virtual DbSet<Transactions> Transactions { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WINMSSQL\\SQLEXPRESS;Initial Catalog=testdb_2023;Persist Security Info=False;User ID=yashvip;Password=yashvip@2023;Connection Timeout=30;");
                //optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__Categori__19093A0B5879791D");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

            });

            /*modelBuilder.Entity<CountryMaster>(entity =>
            {
                entity.HasKey(e => e.IdCountry);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(250);
            });*/



            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__Transact__55433A6BC6671F9A");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity
                    .HasOne(d => d.Categories)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Transacti__Categ__1BC821DD");    
            });

            //modelBuilder.Entity<Transactions>().Ignore(t => t.Categories);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
