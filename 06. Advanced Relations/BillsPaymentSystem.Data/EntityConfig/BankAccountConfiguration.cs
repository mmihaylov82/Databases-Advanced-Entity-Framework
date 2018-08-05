using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using BillsPaymentSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfig
{
    class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            //one-to-one relationship
            builder.HasOne(x => x.PaymentMethod)
                .WithOne(x => x.BankAccount)
                .HasForeignKey<PaymentMethod>(x => x.BankAccountId);

            builder.HasKey(e => e.BankAccountId);

            builder.Property(e => e.Balance)
                .IsRequired();

            builder.Property(e => e.BankName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(e => e.SwiftCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
