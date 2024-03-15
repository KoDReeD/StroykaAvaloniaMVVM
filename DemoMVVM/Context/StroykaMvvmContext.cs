using System;
using System.Collections.Generic;
using DemoMVVM.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVVM.Context;

public partial class StroykaMvvmContext : DbContext
{
    public StroykaMvvmContext()
    {
    }

    public StroykaMvvmContext(DbContextOptions<StroykaMvvmContext> options)
        : base(options)
    {
    }

    private static StroykaMvvmContext _context;

    public static StroykaMvvmContext GetContext()
    {
        if (_context == null)
        {
            _context = new StroykaMvvmContext();
        }

        return _context;
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql("Host=localhost;Database=stroykaMVVM;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(200)
                .HasColumnName("categoryname");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Clientfio).HasColumnName("clientfio");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Orderdeliverydate).HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderpickuppoint_fkey");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderstatus_fkey");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productarticlenumber }).HasName("orderproduct_pkey");

            entity.ToTable("orderproduct");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productamount).HasColumnName("productamount");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Orderstatusid).HasName("orderstatus_pk");

            entity.ToTable("orderstatus");

            entity.Property(e => e.Orderstatusid).HasColumnName("orderstatusid");
            entity.Property(e => e.Orderstatusname)
                .HasMaxLength(100)
                .HasColumnName("orderstatusname");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Organizationid).HasName("organization_pkey");

            entity.ToTable("organization");

            entity.Property(e => e.Organizationid).HasColumnName("organizationid");
            entity.Property(e => e.Organizationname)
                .HasMaxLength(200)
                .HasColumnName("organizationname");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Pickuppointid).HasName("pickuppoint_pk");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Pickuppointid).HasColumnName("pickuppointid");
            entity.Property(e => e.Pickuppointaddres)
                .HasMaxLength(200)
                .HasColumnName("pickuppointaddres");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcategory).HasColumnName("productcategory");
            entity.Property(e => e.Productcost)
                .HasPrecision(19, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturer).HasColumnName("productmanufacturer");
            entity.Property(e => e.Productmaxdiscountamount).HasColumnName("productmaxdiscountamount");
            entity.Property(e => e.Productname).HasColumnName("productname");
            entity.Property(e => e.Productphoto).HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Productunitmeasurement).HasColumnName("productunitmeasurement");
            entity.Property(e => e.Productvendor).HasColumnName("productvendor");

            entity.HasOne(d => d.ProductcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productcategory_fkey");

            entity.HasOne(d => d.ProductmanufacturerNavigation).WithMany(p => p.ProductProductmanufacturerNavigations)
                .HasForeignKey(d => d.Productmanufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productmanufacturer_fkey");

            entity.HasOne(d => d.ProductvendorNavigation).WithMany(p => p.ProductProductvendorNavigations)
                .HasForeignKey(d => d.Productvendor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_productvendor_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pk");

            entity.ToTable("Role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("User");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnName("userid");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany()
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_userrole_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
