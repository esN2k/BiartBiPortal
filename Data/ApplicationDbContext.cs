﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; // ModelBuilder için bu using gerekli olabilir

namespace BiartBiPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // <-- EKLENECEK KISIM BURASI
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Bu satır daima en üstte kalmalı

            // SQLite 'nvarchar(max)' desteklemediği için Identity tablolarını
            // ve diğer potansiyel string'leri manuel olarak yeniden yapılandırıyoruz.
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        // MaxLength belirtilmemiş string'leri (SQL Server'da nvarchar(max) olanları)
                        // SQLite'ın anlayacağı 'TEXT' tipine dönüştür.
                        property.SetColumnType("TEXT");
                    }
                }
            }
        }
        // <-- EKLENECEK KISIM BURADA BİTİYOR
    }
}