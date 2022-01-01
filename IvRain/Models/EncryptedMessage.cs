using MessagePack;

namespace IvRain.Models;

[MessagePackObject]
public class EncryptedMessage
{
    [Key(0)]
    public byte[] EncryptedData { get; set; }
    [Key(1)]
    public byte[] Iv { get; set; }
    [Key(2)]
    public byte[] Salt { get; set; }
}