using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Model.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("City")]
    public class CityEntity
    {
        [StringLength(20)]
        public string Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
