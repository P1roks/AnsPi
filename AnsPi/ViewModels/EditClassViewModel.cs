using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace AnsPi.ViewModels
{
    public partial class EditClassViewModel : ObservableObject, IQueryAttributable 
    {
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        private string _input;

        public EditClassViewModel() 
        {
            Input = null; 
            SaveCommand = new AsyncRelayCommand(Save);
            CancelCommand = new AsyncRelayCommand(Cancel);
        }

        private async Task Save()
        {
            Dictionary<string, object> navi = new()
            {
                {"students", Input}
            };
            await Shell.Current.GoToAsync("..", navi);
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("input"))
            {
                Input = (string)query["input"];
            }
        }
    }
}
