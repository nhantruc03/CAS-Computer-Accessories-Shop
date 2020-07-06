//namespace Model.EF
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    [Table("Category")]
//    public partial class Category
//    {
//        public long ID { get; set; }

//        [StringLength(250)]
//        [Display(Name = "Tên danh mục sản phẩm")]
//        public string Name { get; set; }

//        [StringLength(250)]
//        [Display(Name = "Tên đường dẫn để hiển thị")]
//        public string MetaTitle { get; set; }

//        [Display(Name = "Danh mục cha")]
//        public long? ParentID { get; set; }

//        [Display(Name = "Thứ tự hiển thị")]
//        public int? DisplayOrder { get; set; }

//        [StringLength(250)]
//        [Display(Name = "Tài khoản")]
//        public string SeoTiTle { get; set; }

//        public DateTime? CreateDate { get; set; }

//        [StringLength(50)]
//        public string CreatedBy { get; set; }

//        public DateTime? ModifiedDate { get; set; }

//        [StringLength(50)]
//        public string ModifiedBy { get; set; }

//        public bool Status { get; set; }

//        public bool? ShowOnHome { get; set; }
//    }
//}
