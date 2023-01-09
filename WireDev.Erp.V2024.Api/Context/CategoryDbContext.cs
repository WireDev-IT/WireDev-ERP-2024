using System;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
	public class CategoryDbContext : DbContext
	{
		public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
        {

		}

        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<Category>().HasKey("Uuid");
            _ = builder.Entity<Category>().HasData(new Category() { Description = "Default_Catetgory" });
        }

    }
}

