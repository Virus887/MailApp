﻿using MailApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp.Infrastructure.Mappings
{
    class GroupMap : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(nameof(Group));
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName($"{nameof(Group)}Id");
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.Accounts);
        }
    }
}