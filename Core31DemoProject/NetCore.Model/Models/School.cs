namespace NetCore.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("School")]
    public partial class School
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string SchoolName { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

       // public virtual ICollection<User> Users { get; set; }

    }
}
