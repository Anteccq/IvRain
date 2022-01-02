using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models;
using IvRain.Models.Services;
using Reactive.Bindings;

namespace IvRain.ViewModels
{
    public class InitializeDataPageViewModel
    {
        public ReactiveProperty<bool> IsRegistrable { get; } = new(false);
        public ReactiveProperty<string> FirstPassword { get; }
        public ReactiveProperty<string> SecondPassword { get; }
        public ReactiveProperty<bool> IsRegistered { get; }

        public AsyncReactiveCommand Register { get; }

        public InitializeDataPageViewModel(IBlockService blockService)
        {
            FirstPassword = new ReactiveProperty<string>("");
            SecondPassword = new ReactiveProperty<string>("");
            FirstPassword
                .Subscribe(x =>
                    IsRegistrable.Value = !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == SecondPassword.Value);

            SecondPassword
                .Subscribe(x =>
                    IsRegistrable.Value = !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == FirstPassword.Value);

            IsRegistered = new ReactiveProperty<bool>(false);
            Register = new AsyncReactiveCommand();
            Register
                .WithSubscribe(async _ =>
                {
                    await blockService.SetBlocksAsync(FirstPassword.Value, new List<Block>(), CancellationToken.None);
                    await Task.Delay(500);
                    IsRegistered.Value = true;
                });
        }
    }
}
