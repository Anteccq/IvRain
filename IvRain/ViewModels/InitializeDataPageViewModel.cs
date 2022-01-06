using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models;
using IvRain.Models.Services;
using Reactive.Bindings;

namespace IvRain.ViewModels
{
    public class InitializeDataPageViewModel : INotifyPropertyChanged
    {
        public ReactiveProperty<string> FirstPassword { get; }
        public ReactiveProperty<string> SecondPassword { get; }

        public ReactiveProperty<RegistrationStatus> RegistrationStatusProverProperty { get; } =
            new(RegistrationStatus.UnRegistrable);
        private ReactiveProperty<bool> IsRegistrable { get; }
        public AsyncReactiveCommand Register { get; }

        public InitializeDataPageViewModel(IBlockService blockService)
        {
            FirstPassword = new ReactiveProperty<string>("");
            SecondPassword = new ReactiveProperty<string>("");
            FirstPassword
                .Subscribe(x =>
                    RegistrationStatusProverProperty.Value =
                        !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == SecondPassword.Value
                            ? RegistrationStatus.Registrable
                            : RegistrationStatus.UnRegistrable);

            SecondPassword
                .Subscribe(x =>
                    RegistrationStatusProverProperty.Value =
                        !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == FirstPassword.Value
                            ? RegistrationStatus.Registrable
                            : RegistrationStatus.UnRegistrable);

            IsRegistrable = new ReactiveProperty<bool>();
            
            RegistrationStatusProverProperty.Subscribe(x => IsRegistrable.Value = x == RegistrationStatus.Registrable);
            
            Register = new AsyncReactiveCommand(IsRegistrable);
            Register
                .WithSubscribe(async _ =>
                {
                    await blockService.SetBlocksAsync(FirstPassword.Value, new List<Block>(), CancellationToken.None);
                    await Task.Delay(500);
                    RegistrationStatusProverProperty.Value = RegistrationStatus.Registered;
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
