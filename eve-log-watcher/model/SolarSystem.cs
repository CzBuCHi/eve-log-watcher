using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace eve_log_watcher.model
{
    [Table("eve_map_solarsystems")]
    // ReSharper disable once ClassNeverInstantiated.Global   
    public class SolarSystem
    {
        [Column("solarsystem_id")]
        [Key]
        public int Id { get; set; }

        [Column("region_id")]
        public int RegionId { get; set; }

        [Column("region_name")]
        public string RegionName { get; set; }

        [Column("solarsystem_name")]
        public string SolarsystemName { get; set; }

        [Column("security")]
        public float Security { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }

        [Column("z")]
        public int Z { get; set; }

        [Column("flat_x")]
        public int FlatX { get; set; }

        [Column("flat_y")]
        public int FlatY { get; set; }

        [Column("dotlan_x")]
        public int DotlanX { get; set; }

        [Column("dotlan_y")]
        public int DotlanY { get; set; }

        [Column("has_station")]
        public bool HasStation { get; set; }
    }
}
