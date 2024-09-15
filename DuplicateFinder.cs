using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DuplicateFinder
{
    public class DuplicateFinder : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        public override Guid Id { get; } = Guid.Parse("d85c1809-6ab7-40c2-9434-af74feeae2d3");

        private DuplicateFinderSidebarView _sidebarView;
        private DuplicateFinderSidebarViewModel _sidebarViewModel;

        public DuplicateFinder(IPlayniteAPI api) : base(api)
        {
            Properties = new GenericPluginProperties
            {
                HasSettings = false,
            };

            _sidebarViewModel = new DuplicateFinderSidebarViewModel(this);
            _sidebarViewModel.RequestFindDuplicates += ExecuteDuplicateFinder;
            _sidebarView = new DuplicateFinderSidebarView(_sidebarViewModel);
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return base.GetSettings(firstRunSettings);
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return base.GetSettingsView(firstRunSettings);
        }

        public override Control GetGameViewControl(GetGameViewControlArgs args)
        {
            return base.GetGameViewControl(args);
        }

        public override IEnumerable<SidebarItem> GetSidebarItems()
        {
            yield return new SidebarItem
            {
                Type = SiderbarItemType.View,
                Icon = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/DuplicateFinder;component/icon.png"))
                },
                Title = "DuplicateFinder",
                Visible = true,
                Opened = () => { return _sidebarView; },
            };
        }

        private void ExecuteDuplicateFinder(bool includeHiddenGames, bool checkSimilarity, uint tolerance)
        {
            IEnumerable<Game> searchList;
            List<DuplicateGame> results;

            if (includeHiddenGames)
            {
                searchList = PlayniteApi.Database.Games;
            }
            else
            {
                searchList = PlayniteApi.Database.Games.Where(game => !game.Hidden);
            }

            if (checkSimilarity)
            {
                results = FindSimilars(searchList, tolerance);
            }
            else
            {
                results = FindDuplicates(searchList);
            }

            ShowResultsInWindow(results);
        }

        private List<DuplicateGame> FindDuplicates(IEnumerable<Game> searchList)
        {
            List<DuplicateGame> gamesWithDuplicates = new List<DuplicateGame>();

            foreach (Game candidate in searchList)
            {
                uint reps = 0;

                foreach (Game game in searchList)
                {
                    if (candidate.Name == game.Name)
                    {
                        reps++;
                    }
                }

                if (reps > 1)
                {
                    gamesWithDuplicates.Add(new DuplicateGame
                    (
                        candidate.Id,
                        candidate.Name,
                        candidate.SortingName,
                        candidate.Icon,
                        candidate.Hidden,
                        candidate.PluginId
                    ));
                }
            }

            gamesWithDuplicates.Sort(new GameAlphabeticalComparer<DuplicateGame>());

            return gamesWithDuplicates;
        }

        private List<DuplicateGame> FindSimilars(IEnumerable<Game> searchList, uint tolerance)
        {
            List<DuplicateGame> gamesWithDuplicates = new List<DuplicateGame>();

            foreach (Game candidate in searchList)
            {
                uint reps = 0;

                // Levenshtein Option 1
                //foreach (Game game in searchList)
                //{
                //    if (Fastenshtein.Levenshtein.Distance(candidate.Name, game.Name) <= tolerance)
                //    {
                //        reps++;
                //    }
                //}
                
                // Levenshtein Option 2 : Quicker, less memory allocations, not thread-safe
                Fastenshtein.Levenshtein levenshtein = new Fastenshtein.Levenshtein(candidate.Name);
                foreach (Game game in searchList)
                {
                    if (levenshtein.DistanceFrom(game.Name) <= tolerance)
                    {
                        reps++;
                    }
                }

                if (reps > 1)
                {
                    gamesWithDuplicates.Add(new DuplicateGame
                    (
                        candidate.Id,
                        candidate.Name,
                        candidate.SortingName,
                        candidate.Icon,
                        candidate.Hidden,
                        candidate.PluginId
                    ));
                }
            }

            gamesWithDuplicates.Sort(new GameAlphabeticalComparer<DuplicateGame>());

            return gamesWithDuplicates;
        }

        private void ShowResultsInWindow(List<DuplicateGame> results)
        {
            if (true)
            {
                // TODO : Log results
                foreach (DuplicateGame game in results)
                {
                    logger.Debug($"{game.Name} || {game.Library}");
                }
            }

            _sidebarViewModel.FoundDuplicates = results;
        }

        public class DuplicateGame
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string SortingName { get; set; }
            public string Icon { get; set; }
            public bool IsHidden { get; set; }
            public string Library { get; set; }
            public object IconImage
            {
                get
                {
                    if (string.IsNullOrEmpty(Icon))
                    {
                        return ResourceProvider.GetResource("DefaultGameIcon");
                    }
                    else
                    {
                        return Playnite.SDK.API.Instance.Database.GetFullFilePath(Icon);
                    }
                }
            }

            public DuplicateGame(Guid id, string name, string sortingName, string icon, bool isHidden, Guid libraryPluginId)
            {
                Id = id;
                Name = name;
                SortingName = sortingName;
                Icon = icon;
                IsHidden = isHidden;

                LibraryPlugin libraryPlugin = Playnite.SDK.API.Instance.Addons.Plugins.FirstOrDefault(plugin => plugin.Id == libraryPluginId) as LibraryPlugin;
                if (libraryPlugin == null)
                {
                    Library = ResourceProvider.GetString("LOCDuplicateFinder_NoLibraryPlugin");
                }
                else
                {
                    Library = libraryPlugin.Name;
                }
            }
        }

        class GameAlphabeticalComparer<T> : IComparer<T> where T : DuplicateGame
        {
            public int Compare(T x, T y)
            {
                string a = x.SortingName ?? x.Name;
                string b = y.SortingName ?? y.Name;

                int result = a.CompareTo(b);
                if (result == 0)
                {
                    return 1;
                }
                else
                {
                    return result;
                }
            }
        }
    }
}