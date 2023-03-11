using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cps_x32.Models
{
    [Table("weightstation")]
    public partial class WeightStation
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Description { get; set; }
        public virtual ICollection<LotNumber> LotNumbers { get; set; }
    }
}
