using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models;
using IvRain.Models.Services;
using IvRain.Models.Storage;
using MessagePack;
using Reactive.Bindings;

namespace IvRain.ViewModels;

public class AuthPageViewModel
{
    private readonly IStorage _storage;
    private readonly IBlockService _blockService;
    public ReactiveProperty<string> InputPassword { get; } = new("");
    public ReactiveProperty<string> ErrorMessage { get; } = new("");
    public ReactiveProperty<bool> IsAuthenticated { get; }
    public List<Block> Block { get; private set; }

    public AuthPageViewModel(IBlockService blockService, IStorage storage)
    {
        IsAuthenticated = new ReactiveProperty<bool>(false);
        _storage = storage;
        _blockService = blockService;
;    }

    public async ValueTask ChallengeAuthenticationAsync()
    {
        if (IsAuthenticated.Value) return;

        try
        {
            Block = await _blockService.GetBlocksAsync(InputPassword.Value);
            IsAuthenticated.Value = true;
        }
        catch (Exception e) when (e is MessagePackSerializationException or CryptographicException)
        {
            ErrorMessage.Value = "Invalid password.";
        }
        catch(Exception e)
        {
            // とりあえず例外のメッセージを投げておく。
            ErrorMessage.Value = e.Message;
        }
    }

    public async ValueTask<bool> IsFirstBoot()
        => !(await _storage.IsExistDataAsync(CancellationToken.None));
}