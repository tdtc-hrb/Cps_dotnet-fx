using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cps_x32.Models
{
    [Table("dispatchtemporary")]
    public partial class DispatchStore
    {
        [Column("total_weight")]
        public decimal TotalWeight { get; set; }
        public decimal Weight { get; set; }
        [StringLength(25), Column("car_model")]
        public string CarModel { get; set; }
        [StringLength(25), Column("car_number")]
        public string CarNumber { get; set; }
        [Column("carry_weight")]
        public decimal CarryWeight { get; set; }
        [Column("self_weight")]
        public decimal SelfWeight { get; set; }
        [Column("pl_weight")]
        public decimal PlWeight { get; set; }
        [Column("breed_id")]
        public int BreedCoalId { get; set; }
        [Column("past_date")]
        public DateTime PastDate { get; set; }
        [StringLength(25), Column("past_time")]
        public string PastTime { get; set; }
        [Column("arrive_id")]
        public int ArriveStationId { get; set; }
        public int Consist { get; set; }
        [Column("lot_id")]
        public int LotNumberId { get; set; }
        [Column("is_obsoleted")]
        public Boolean IsObsoleted { get; set; }

        public virtual BreedCoal BreedCoal { get; set; }
        public virtual ArriveStation ArriveStation { get; set; }
        public virtual LotNumber LotNumber { get; set; }
    }
}
