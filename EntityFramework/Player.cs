using System;
using System.Collections.Generic;

namespace FFBDraftAPI.EntityFramework;

public partial class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Rank { get; set; }

    public int? Nflteam { get; set; }

    public int? Position { get; set; }

    public int? ByeWeek { get; set; }

    public Guid? Ffbteam { get; set; }

    public int? Year { get; set; }
}
