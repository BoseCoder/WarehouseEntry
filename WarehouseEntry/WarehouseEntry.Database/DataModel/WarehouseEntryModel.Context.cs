﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WarehouseEntry.Database.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WarehouseEntryDbContext : DbContext
    {
        public WarehouseEntryDbContext()
            : base("name=WarehouseEntryDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<EntryRecord> EntryRecord { get; set; }
        public DbSet<FlowProcess> FlowProcess { get; set; }
        public DbSet<FlowTask> FlowTask { get; set; }
        public DbSet<SecurityRole> SecurityRole { get; set; }
        public DbSet<SecurityUser> SecurityUser { get; set; }
        public DbSet<SystemMenu> SystemMenu { get; set; }
        public DbSet<SystemRight> SystemRight { get; set; }
    }
}
