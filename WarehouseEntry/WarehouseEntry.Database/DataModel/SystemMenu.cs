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
    
    public partial class SystemMenu
    {
        public SystemMenu()
        {
            this.SubMenus = new HashSet<SystemMenu>();
            this.SystemRights = new HashSet<SystemRight>();
        }
    
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public bool Enabled { get; set; }
    
        public virtual ICollection<SystemMenu> SubMenus { get; set; }
        public virtual SystemMenu ParentMenu { get; set; }
        public virtual ICollection<SystemRight> SystemRights { get; set; }
    }
}
