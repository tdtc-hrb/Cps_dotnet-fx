using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cps_x32.Models
{
    [Table("lotnumstatuses")]
    public partial class LotnumStatus
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(120)]
        public string Description { get; set; }
        public virtual ICollection<LotNumber> LotNumbers { get; set; }
    }
}
