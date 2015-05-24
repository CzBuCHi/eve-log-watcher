using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace eve_log_watcher.model
{
    [Table("eve_map_solarsystem_jumps")]
    // ReSharper disable once ClassNeverInstantiated.Global    
    public class SolarSystemJump
    {
        [Column("from_solarsystem_id", Order = 0)]
        [Key]
        public int FromSolarsystemId { get; set; }

        [Column("to_solarsystem_id", Order = 1)]
        [Key]
        public int ToSolarsystemId { get; set; }

        [Column("from_region_id")]
        public int FromRegionId { get; set; }

        [Column("to_region_id")]
        public int ToRegionId { get; set; }
    }
}
