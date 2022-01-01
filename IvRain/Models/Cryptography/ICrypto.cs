using System.Threading;
using System.Threading.Tasks;

namespace IvRain.Models.Cryptography;

public interface ICrypto
{
    public ValueTask<EncryptedMessage> EncryptAsync(string password, byte[] rawData, CancellationToken cancellationToken = default);
    public ValueTask<byte[]> DecryptAsync(string password, EncryptedMessage message, CancellationToken cancellationToken = default);
}