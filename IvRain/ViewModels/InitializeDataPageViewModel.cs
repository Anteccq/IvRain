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

        public InitializeDataPageViewModel()
        {
            FirstPassword = new ReactiveProperty<string>("");
            SecondPassword = new ReactiveProperty<string>("");
            FirstPassword
                .Subscribe(x =>
                    IsRegistrable.Value = !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == SecondPassword.Value);

            SecondPassword
                .Subscribe(x =>
                    IsRegistrable.Value = !string.IsNullOrWhiteSpace(x) && x.Length > 5 && x == FirstPassword.Value);
        }
    }
}
