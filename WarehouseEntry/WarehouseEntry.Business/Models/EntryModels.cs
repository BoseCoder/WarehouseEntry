using LinqToExcel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using WarehouseEntry.Business.Languages;

namespace WarehouseEntry.Business.Models
{
    public class ExcelColumnNameAttribute : Attribute
    {
        public string Name { get; set; }

        public static void AddMapping(ExcelQueryFactory factory, Type type)
        {
            if (factory != null && type != null)
            {
                IEnumerable<KeyValuePair<string, object[]>> properties = type.GetProperties().Select(p =>
                    new KeyValuePair<string, object[]>(p.Name,
                        p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), false)))
                    .Where(p => p.Value.Length > 0);
                foreach (KeyValuePair<string, object[]> property in properties)
                {
                    factory.AddMapping(property.Key, ((ExcelColumnNameAttribute)(property.Value[0])).Name);
                }
            }
        }
    }

    [Serializable]
    [DataContract]
    public class EntryCompleteModel : EntryBaseModel
    {
        /// <summary>
        /// 完工日期
        /// </summary>
        [DataMember(Name = "InspectionDate", IsRequired = true)]
        [ExcelColumnName(Name = "完工日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "CompletionDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "CompletionDateRequired")]
        public DateTime CompletionDate { get; set; }
        /// <summary>
        /// 发运日期
        /// </summary>
        [DataMember(Name = "InspectionDate", IsRequired = true)]
        [ExcelColumnName(Name = "发运日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "DespatchDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "DespatchDateRequired")]
        public DateTime DespatchDate { get; set; }

        public EntryCompleteModel()
        {
            DateTime now = DateTime.Now;
            this.CompletionDate = now;
            this.DespatchDate = now;
        }
    }

    [Serializable]
    [DataContract]
    public class EntryBaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "ModelId", IsRequired = true)]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ModelId")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ModelIdRequired")]
        public long ModelId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember(Name = "ProjectName", IsRequired = true)]
        [ExcelColumnName(Name = "项目名称")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ProjectName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProjectNameRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProjectNameStringLength")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目工号
        /// </summary>
        [DataMember(Name = "ProjectNum", IsRequired = true)]
        [ExcelColumnName(Name = "项目工号")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ProjectNum")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProjectNumRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProjectNumStringLength")]
        public string ProjectNum { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Name = "ProductName", IsRequired = true)]
        [ExcelColumnName(Name = "产品名称")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ProductName")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProductNameRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProductNameStringLength")]
        public string ProductName { get; set; }
        /// <summary>
        /// 套数
        /// </summary>
        [DataMember(Name = "SuiteCount", IsRequired = true)]
        [ExcelColumnName(Name = "套数")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "SuiteCount")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "SuiteCountRequired")]
        public int SuiteCount { get; set; }
        /// <summary>
        /// 产品图号
        /// </summary>
        [DataMember(Name = "ProductImgNum", IsRequired = true)]
        [ExcelColumnName(Name = "产品图号")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ProductImgNum")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProductImgNumRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ProductImgNumStringLength")]
        public string ProductImgNum { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember(Name = "Sequence", IsRequired = true)]
        [ExcelColumnName(Name = "序号")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "Sequence")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "SequenceRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "SequenceStringLength")]
        public string Sequence { get; set; }
        /// <summary>
        /// 图号
        /// </summary>
        [DataMember(Name = "ImgNum", IsRequired = true)]
        [ExcelColumnName(Name = "图号")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "ImgNum")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ImgNumRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "ImgNumStringLength")]
        public string ImgNum { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        [DataMember(Name = "Height", IsRequired = true)]
        [ExcelColumnName(Name = "高度")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "Height")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "HeightRequired")]
        public decimal Height { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [DataMember(Name = "Width", IsRequired = true)]
        [ExcelColumnName(Name = "宽度")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "Width")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "WidthRequired")]
        public decimal Width { get; set; }
        /// <summary>
        /// 腹厚
        /// </summary>
        [DataMember(Name = "StomachWeight", IsRequired = true)]
        [ExcelColumnName(Name = "腹厚")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "StomachWeight")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "StomachWeightRequired")]
        public decimal StomachWeight { get; set; }
        /// <summary>
        /// 翼厚
        /// </summary>
        [DataMember(Name = "WingWeight", IsRequired = true)]
        [ExcelColumnName(Name = "翼厚")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "WingWeight")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "WingWeightRequired")]
        public decimal WingWeight { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        [DataMember(Name = "Length", IsRequired = true)]
        [ExcelColumnName(Name = "长度")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "Length")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "LengthRequired")]
        public decimal Length { get; set; }
        /// <summary>
        /// 件数（套）
        /// </summary>
        [DataMember(Name = "PieceCount", IsRequired = true)]
        [ExcelColumnName(Name = "件数")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "PieceCount")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "PieceCountRequired")]
        public int PieceCount { get; set; }
        /// <summary>
        /// 单重
        /// </summary>
        [DataMember(Name = "Weight", IsRequired = true)]
        [ExcelColumnName(Name = "单重")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "Weight")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "WeightRequired")]
        public decimal Weight { get; set; }
        /// <summary>
        /// 组立日期
        /// </summary>
        [DataMember(Name = "AssemblageDate", IsRequired = true)]
        [ExcelColumnName(Name = "组立日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "AssemblageDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "AssemblageDateRequired")]
        public DateTime AssemblageDate { get; set; }
        /// <summary>
        /// 焊接日期
        /// </summary>
        [DataMember(Name = "SolderingDate", IsRequired = true)]
        [ExcelColumnName(Name = "焊接日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "SolderingDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "SolderingDateRequired")]
        public DateTime SolderingDate { get; set; }
        /// <summary>
        /// 矫正日期
        /// </summary>
        [DataMember(Name = "CorrectionDate", IsRequired = true)]
        [ExcelColumnName(Name = "矫正日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "CorrectionDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "CorrectionDateRequired")]
        public DateTime CorrectionDate { get; set; }
        /// <summary>
        /// 报检日期
        /// </summary>
        [DataMember(Name = "InspectionDate", IsRequired = true)]
        [ExcelColumnName(Name = "报检日期")]
        [Display(ResourceType = typeof(EntryModelResource), Name = "InspectionDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(EntryModelResource), ErrorMessageResourceName = "InspectionDateRequired")]
        public DateTime InspectionDate { get; set; }

        public EntryBaseModel()
        {
            DateTime now = DateTime.Now;
            this.AssemblageDate = now;
            this.SolderingDate = now;
            this.CorrectionDate = now;
            this.InspectionDate = now;
        }
    }
}