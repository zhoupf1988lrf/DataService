namespace NetCore.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [Required]
       // [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Account { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [StringLength(10)]
        public string Email { get; set; }

        [StringLength(11)]
        public string Mobile { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [StringLength(10)]
        public string CompanyName { get; set; }

        public int Status { get; set; }

        public int UserType { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public int? LastModifierId { get; set; }

        public DateTime? LastModifierTime { get; set; }

        public virtual Company Company { get; set; }
        /// <summary>
        /// GraduationSchoolId   ÊµÀýÃû³Æ+Id
        /// </summary>
        public int GraduationSchoolId { get; set; }
        //public virtual School GraduationSchool { get; set; }
    }
}
