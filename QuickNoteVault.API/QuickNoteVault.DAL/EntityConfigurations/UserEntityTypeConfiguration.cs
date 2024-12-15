﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickNoteVault.DAL.Entities;

namespace QuickNoteVault.DAL.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

            builder.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(256);

            builder.Property(u => u.LastName)
                   .HasMaxLength(50);

            builder.HasMany(u => u.Notes)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email)
                  .IsUnique();
        }
    }
}
