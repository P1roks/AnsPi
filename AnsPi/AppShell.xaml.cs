using AnsPi.Views;

namespace AnsPi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditClassPage), typeof(EditClassPage));
        }
    }
}