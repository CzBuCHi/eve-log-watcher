using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace eve_log_watcher.model
{
    [Table("eve_map_regions")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Region
    {
        [Column("region_id", Order = 0)]
        [Key]
        public int Id { get; set; }

        [Column("region_name", Order = 1)]
        public string Name { get; set; }
    }
}
