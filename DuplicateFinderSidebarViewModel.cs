using System;
using System.Collections.Generic;

namespace DuplicateFinder
{
    public class DuplicateFinderSidebarViewModel : ObservableObject
    {
        public Action<bool, bool, uint> RequestFindDuplicates;

        public List<DuplicateFinder.DuplicateGame> FoundDuplicates { get => _foundDuplicates; set => SetValue(ref _foundDuplicates, value);  }
        public bool IncludeHiddenGames { get => _includeHiddenGames; set => SetValue(ref _includeHiddenGames, value); }
        public bool CheckSimilarity { get => _checkSimilarity; set => SetValue(ref _checkSimilarity, value); }
        public uint Tolerance { get => _tolerance; set => SetValue(ref _tolerance, value); }

        private List<DuplicateFinder.DuplicateGame> _foundDuplicates;
        private bool _includeHiddenGames = false;
        private bool _checkSimilarity = false;
        private uint _tolerance = 0;
        private DuplicateFinder _plugin;

        public DuplicateFinderSidebarViewModel(DuplicateFinder plugin)
        {
            _foundDuplicates = new List<DuplicateFinder.DuplicateGame>();
            _plugin = plugin;
        }

        public void OnFindButtonClicked()
        {
            RequestFindDuplicates?.Invoke(_includeHiddenGames, _checkSimilarity, _tolerance);
        }
    }
}
