using System.Threading;
using System.Threading.Tasks;

namespace IvRain.Models.Storage;

public interface IStorage
{
    ValueTask<bool> IsExistDataAsync(CancellationToken cancellationToken);
    ValueTask<EncryptedMessage> ReadAsync(CancellationToken cancellationToken);
    ValueTask DeleteAsync(CancellationToken cancellationToken);
    ValueTask WriteAsync(EncryptedMessage message, CancellationToken cancellationToken);
}