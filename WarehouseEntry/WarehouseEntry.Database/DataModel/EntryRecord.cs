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
    
    public partial class EntryRecord
    {
        public long RecordID { get; set; }
        public System.DateTime CreationTime { get; set; }
        public Nullable<System.DateTime> CompletionTime { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNum { get; set; }
        public string ProductName { get; set; }
        public int SuiteCount { get; set; }
        public string ProductImgNum { get; set; }
        public string Sequence { get; set; }
        public string ImgNum { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal StomachWeight { get; set; }
        public decimal WingWeight { get; set; }
        public decimal Length { get; set; }
        public int PieceCount { get; set; }
        public decimal Weight { get; set; }
        public System.DateTime AssemblageDate { get; set; }
        public System.DateTime SolderingDate { get; set; }
        public System.DateTime CorrectionDate { get; set; }
        public System.DateTime InspectionDate { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public Nullable<System.DateTime> DespatchDate { get; set; }
    
        public virtual SecurityUser Creator { get; set; }
        public virtual FlowProcess FlowProcess { get; set; }
        public virtual SecurityUser Completer { get; set; }
    }
}
