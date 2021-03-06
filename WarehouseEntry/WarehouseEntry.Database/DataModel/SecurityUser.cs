//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class SecurityUser
    {
        public SecurityUser()
        {
            this.CreatedEntryRecords = new HashSet<EntryRecord>();
            this.CompletedEntryRecords = new HashSet<EntryRecord>();
            this.HandlingProcesses = new HashSet<FlowProcess>();
            this.HandledTasks = new HashSet<FlowTask>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public System.DateTime CreationTime { get; set; }
    
        public virtual ICollection<EntryRecord> CreatedEntryRecords { get; set; }
        public virtual ICollection<EntryRecord> CompletedEntryRecords { get; set; }
        public virtual ICollection<FlowProcess> HandlingProcesses { get; set; }
        public virtual ICollection<FlowTask> HandledTasks { get; set; }
        public virtual SecurityRole SecurityRole { get; set; }
    }
}
