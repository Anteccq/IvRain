using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Reactive.Bindings.Extensions;

namespace IvRain.ViewModels
{
    public class PasswordManagePageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Block> Blocks { get; }
        public ObservableCollection<Block> FilteredBlockList { get; set; }
        private readonly string _password;
        private readonly IBlockService _blockService;
        public ReactiveCommand StoreBlock { get; }
        public ReactiveCommand FilterBlocks { get; }
        public ReactiveProperty<string> SearchText { get; set; }

        public PasswordManagePageViewModel(IBlockService service,  IEnumerable<Block> blocks, string password)
        {
            var collection = blocks.ToList();
            Blocks = new ObservableCollection<Block>(collection);
            FilteredBlockList = new ObservableCollection<Block>(collection);
            _blockService = service;
            _password = password;
            SearchText = new ReactiveProperty<string>("");
            Blocks.CollectionChangedAsObservable()
                .Subscribe(async _ => await _blockService.SetBlocksAsync(_password, Blocks.ToList()));
            StoreBlock = new ReactiveCommand();
            StoreBlock.Subscribe(async _ => await _blockService.SetBlocksAsync(_password, Blocks.ToList(), CancellationToken.None));
            SearchText.Subscribe(_ => FilterBlockList());
            FilterBlocks = new ReactiveCommand();
            FilterBlocks.Subscribe(_ => FilterBlockList());
            Blocks.CollectionChanged += (_, _) => FilterBlockList();
        }

        private void FilterBlockList()
        {
            var searchResult = SearchText.Value is "" or null
            ? Blocks
            : Blocks.Where(x => x.SiteName.StartsWith(SearchText.Value, StringComparison.OrdinalIgnoreCase));

            FilteredBlockList.Clear();
            foreach (var block in searchResult)
                FilteredBlockList.Add(block);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
