using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using BillsPaymentSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfig
{
    class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            //one-to-one relationship
            builder.HasOne(x => x.PaymentMethod)
                .WithOne(x => x.CreditCard)
                .HasForeignKey<PaymentMethod>(x => x.CreditCardId);

            builder.HasKey(e => e.CreditCardId);

            builder.Property(e => e.Limit)
                .IsRequired();

            builder.Property(e => e.MoneyOwed)
                .IsRequired();

            builder.Property(e => e.ExpirationDate)
                .IsRequired();

            builder.Ignore(e => e.LimitLeft);
        }
    }
}
