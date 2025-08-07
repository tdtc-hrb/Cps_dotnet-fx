using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cps_x32.Models
{
    [Table("lotnumber")]
    public partial class LotNumber
    {
        public int Id { get; set; }
        [Column("weightStation_id")]
        public int WeightStationId { get; set; }
        [Column("status_id")]
        public int LotnumStatusId { get; set; }
        [Column("coupler_number")]
        public int CouplerNumber { get; set; }

        public virtual WeightStation WeightStation { get; set; }
        public virtual LotnumStatus LotnumStatus { get; set; }
        public virtual ICollection<DispatchStore> DispatchStores { get; set; }
    }
}
