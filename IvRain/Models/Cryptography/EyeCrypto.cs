using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace IvRain.Models.Cryptography;

public class EyeCrypto : ICrypto
{
    private const int SaltSize = 16;
    private const int KeySize = 16;

    public async ValueTask<byte[]> DecryptAsync(string password, EncryptedMessage message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var deriveBytes = new Rfc2898DeriveBytes(password, message.Salt);
        using var decAlg = Aes.Create();
        decAlg.Key = deriveBytes.GetBytes(KeySize);
        decAlg.Padding = PaddingMode.PKCS7;
        decAlg.IV = message.Iv;
        await using var ms = new MemoryStream();
        await using var cs = new CryptoStream(ms, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
        await cs.WriteAsync(message.EncryptedData, cancellationToken);
        await cs.FlushFinalBlockAsync(cancellationToken);
        return ms.ToArray();
    }

    public async ValueTask<EncryptedMessage> EncryptAsync(string password, byte[] rawData, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize);
        var salt = deriveBytes.Salt;
        using var encAlg = Aes.Create();
        encAlg.Key = deriveBytes.GetBytes(KeySize);
        encAlg.Padding = PaddingMode.PKCS7;
        await using var ms = new MemoryStream();
        await using var cs = new CryptoStream(ms, encAlg.CreateEncryptor(), CryptoStreamMode.Write);
        await cs.WriteAsync(rawData, cancellationToken);
        await cs.FlushFinalBlockAsync(cancellationToken);
        var message = new EncryptedMessage
        {
            EncryptedData = ms.ToArray(),
            Iv = encAlg.IV,
            Salt = salt
        };
        return message;
    }
}