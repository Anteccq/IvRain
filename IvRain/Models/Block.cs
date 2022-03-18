using MessagePack;

namespace IvRain.Models;

[MessagePackObject]
public class Block
{
    [Key(0)]
    public string SiteName { get; set; } = "";
    [Key(1)]

    public string Password { get; set; } = "";
}