using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFBDraftAPI.EntityFramework;

[Table("Draft")]
public partial class Draft
{
    [Key]
    public Guid Id { get; set; }

    public int DraftNumber { get; set; }

    public Guid? PlayerId { get; set; }

    [Column("FFBTeamId")]
    public Guid? FfbteamId { get; set; }

    public int Year { get; set; }
}
