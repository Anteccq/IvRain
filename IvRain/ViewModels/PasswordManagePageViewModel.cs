using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models;
using IvRain.Models.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace IvRain.ViewModels
{
    public class PasswordManagePageViewModel
    {
        public ObservableCollection<Block> Blocks { get; }
        private readonly string _password;
        private readonly IBlockService _blockService;
        public ReactiveCommand StoreBlock;

        public PasswordManagePageViewModel(IBlockService service,  IEnumerable<Block> blocks, string password)
        {
            Blocks = new ObservableCollection<Block>(blocks);
            _blockService = service;
            _password = password;
            Blocks.CollectionChangedAsObservable()
                .Subscribe(async _ => await _blockService.SetBlocksAsync(_password, Blocks.ToList()));
            StoreBlock = new ReactiveCommand();
            StoreBlock.Subscribe(_ => _blockService.SetBlocksAsync(_password, Blocks.ToList(), CancellationToken.None));
        }
    }
}
