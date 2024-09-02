using System;
using System.Collections.Generic;

namespace FFBDraftAPI.EntityFramework;

public partial class Ffbteam
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Manager { get; set; } = null!;

    public string? Email { get; set; }

    public string? ThirdPartyId { get; set; }

    public string? Nickname { get; set; }
}
