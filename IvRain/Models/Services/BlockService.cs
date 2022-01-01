using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models.Cryptography;
using IvRain.Models.Storage;
using MessagePack;

namespace IvRain.Models.Services;

public class BlockService : IBlockService
{
    private readonly ICrypto _crypto;
    private readonly IStorage _storage;

    public BlockService(ICrypto crypto, IStorage storage)
    {
        _crypto = crypto;
        _storage = storage;
    }

    public async ValueTask<List<Block>> GetBlocksAsync(string password, CancellationToken cancellationToken = default)
    {
        var message = await _storage.ReadAsync(cancellationToken);
        var decryptedData = await _crypto.DecryptAsync(password, message, cancellationToken);
        await using var ms = new MemoryStream();
        await ms.WriteAsync(decryptedData.AsMemory(0, decryptedData.Length), cancellationToken);
        ms.Position = 0;
        var blocks = await MessagePackSerializer.DeserializeAsync<List<Block>>(ms, cancellationToken: cancellationToken);
        return blocks;
    }

    public async ValueTask SetBlocksAsync(string password, List<Block> blocks, CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream();
        await MessagePackSerializer.SerializeAsync(ms, blocks, cancellationToken: cancellationToken);
        var encryptedData = await _crypto.EncryptAsync(password, ms.ToArray(), cancellationToken);
        await _storage.WriteAsync(encryptedData, cancellationToken);
    }
}