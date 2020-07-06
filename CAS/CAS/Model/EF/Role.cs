namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên quyền")]
        public string Name { get; set; }
    }
}
