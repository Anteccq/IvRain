using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace IvRain.Models.Storage;

public class FileStorage : IStorage
{
    private const string FileName = "Kusanagi";
    private readonly string _directoryName;
    private readonly string _filePath;

    public FileStorage()
    {
        _directoryName = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Anteccq/IvRain";
        _filePath = $"{_directoryName}/{FileName}";
    }

    public ValueTask<bool> IsExistDataAsync(CancellationToken cancellationToken)
        => ValueTask.FromResult(File.Exists(_filePath));

    public async ValueTask<EncryptedMessage> ReadAsync(CancellationToken cancellationToken)
    {
        await using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        var message = await MessagePackSerializer.DeserializeAsync<EncryptedMessage>(fs, cancellationToken: cancellationToken);
        return message;
    }

    public ValueTask DeleteAsync(CancellationToken cancellationToken)
    {
        File.Delete(_filePath);
        return ValueTask.CompletedTask;
    }

    public async ValueTask WriteAsync(EncryptedMessage message, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_directoryName))
            Directory.CreateDirectory(_directoryName);

        await using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        await MessagePackSerializer.SerializeAsync(fs, message, cancellationToken: cancellationToken);
    }
}