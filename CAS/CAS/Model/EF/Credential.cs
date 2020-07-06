namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Credential")]
    [Serializable]
    public partial class Credential
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [Display(Name = "Nhóm người dùng")]
        public string UserGroupID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        [Display(Name = "Quyền")]
        public string RoleID { get; set; }
    }
}
