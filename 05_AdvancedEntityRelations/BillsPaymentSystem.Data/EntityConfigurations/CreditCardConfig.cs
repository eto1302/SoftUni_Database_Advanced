using System;
using System.Collections.Generic;
using System.Text;
using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(c => c.CreditCardId);

            builder.Property(c => c.Limit).IsRequired();

            builder.Property(c => c.MoneyOwed).IsRequired();

            builder.Ignore(c => c.LimitLeft);

            builder.Property(c => c.ExpirationDate).IsRequired();
        }
    }
}
