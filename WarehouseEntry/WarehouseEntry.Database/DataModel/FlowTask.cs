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
    
    public partial class FlowTask
    {
        public long TaskID { get; set; }
        public string SectionName { get; set; }
        public string Name { get; set; }
        public System.DateTime HandleTime { get; set; }
        public string HandleUrl { get; set; }
    
        public virtual FlowProcess FlowProcess { get; set; }
        public virtual SecurityUser Handler { get; set; }
    }
}
