using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace IvRain.ViewModels
{
    public class InitializeDataPageViewModel
    {
        public ReactiveProperty<bool> IsRegistrable { get; } = new(false);
        public ReactiveProperty<string> FirstPassword { get; }
        public ReactiveProperty<string> SecondPassword { get; }
        public const string InformationText = "Must be at least 5 characters long.";

        public InitializeDataPageViewModel()
        {
            FirstPassword = new ReactiveProperty<string>("");
            SecondPassword = new ReactiveProperty<string>("");
            FirstPassword
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length < 5 && x == SecondPassword.Value)
                .Subscribe(_ => IsRegistrable.Value = true);

            SecondPassword
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length < 5 && x == FirstPassword.Value)
                .Subscribe(_ => IsRegistrable.Value = true);
        }
    }
}
