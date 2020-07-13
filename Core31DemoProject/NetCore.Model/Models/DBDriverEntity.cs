using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Model.Models
{
    public class DBDriverEntity
    {
        [StringLength(32)]
        public string Id { get; set; }
        [StringLength(50)]
        public string DatabaseDrive { get; set; }
    }
}
