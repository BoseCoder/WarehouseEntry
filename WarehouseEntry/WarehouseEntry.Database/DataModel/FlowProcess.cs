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
    
    public partial class FlowProcess
    {
        public FlowProcess()
        {
            this.EntryRecords = new HashSet<EntryRecord>();
            this.FlowTasks = new HashSet<FlowTask>();
        }
    
        public long ProcessID { get; set; }
        public string Name { get; set; }
        public System.DateTime SetupTime { get; set; }
        public Nullable<System.DateTime> CompleteTime { get; set; }
        public int Status { get; set; }
        public string CurrentTaskSection { get; set; }
        public string CurrentHandleUrl { get; set; }
    
        public virtual ICollection<EntryRecord> EntryRecords { get; set; }
        public virtual SecurityUser CurrentHandler { get; set; }
        public virtual ICollection<FlowTask> FlowTasks { get; set; }
    }
}
