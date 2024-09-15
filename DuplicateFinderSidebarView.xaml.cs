using System.Windows;
using System.Windows.Controls;

namespace DuplicateFinder
{
    public partial class DuplicateFinderSidebarView : UserControl
    {
        public DuplicateFinderSidebarView(DuplicateFinderSidebarViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void OnFindButtonClicked(object sender, RoutedEventArgs e)
        {
            (DataContext as DuplicateFinderSidebarViewModel).OnFindButtonClicked();
        }
    }
}
