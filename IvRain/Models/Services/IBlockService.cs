using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IvRain.Models.Services;

internal interface IBlockService
{
    public ValueTask<List<Block>> GetBlocksAsync(string password, CancellationToken cancellationToken = default);

    public ValueTask SetBlocksAsync(string password, List<Block> blocks, CancellationToken cancellationToken = default);
}