using CommunityToolkit.Mvvm.Input;

namespace AnsPi.ViewModels
{
    public partial class ClassViewModel : IQueryAttributable
    {
        public Models.StudentsHandler Students { get; private set; } = new();
        private uint luckyNumber = Utils.GetLuckyNumber();

        private async Task<bool> CheckClass()
        {
            if(Students.className == null)
            {
                await Shell.Current.DisplayAlert("Message", "Please select a class first!", "OK");
                return false;
            }
            return true; 
        }

        [RelayCommand]
        private async Task ChangeClass()
        {
            string[] classes = Models.StudentsHandler.GetClasses();
            string selected = await Shell.Current.DisplayActionSheet("Choose class", "Cancel", null, classes);
            if(string.IsNullOrEmpty(selected) || selected == "Cancel") return;

            Students.ChangeClass(selected);
        }

        [RelayCommand]
        private async Task AddClass()
        {
            string className = await Shell.Current.DisplayPromptAsync("Add Class", "Enter new class name: ");
            if(!string.IsNullOrWhiteSpace(className) && className.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
            {
                if(!Students.ChangeClass(className))
                {
                    await Shell.Current.GoToAsync(nameof(Views.EditClassPage));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Message", "Provided class already exists! Switching to it", "OK");
                }
            }
        }

        [RelayCommand]
        private async Task EditClass()
        {
            if (!await CheckClass()) return;
            Dictionary<string, object> input = new()
            {
                { "input", string.Join('\r', Students.Select(student => student.FullName)) }
            };
            await Shell.Current.GoToAsync(nameof(Views.EditClassPage), input);
        }

        [RelayCommand]
        private async Task ChangeLuckyNumber()
        {
            if (!await CheckClass()) return;
            Students[0].Present = false;

            string newLucky =
                await Shell.Current.DisplayPromptAsync("Lucky Number", $"Current Lucky Number is: {luckyNumber}", "OK", "Random");

            if(newLucky == null)
            {
                luckyNumber = Students.GetRandomStudentNumber();
            }
            else if(newLucky.Length != 0)
            {
                luckyNumber = uint.TryParse(newLucky, out var lucky) ? lucky : luckyNumber;
            }

            Utils.SetLuckyNumber(luckyNumber);
        }

        [RelayCommand]
        private async Task RollStudent()
        {
            if (!await CheckClass()) return;

            try
            {
                var chosen = Students.RollStudent(luckyNumber);
                await Shell.Current.DisplayAlert("Roll", $"{chosen.FullName} has been selected for answer!", "OK");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Roll", "No student can be selected!", "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.ContainsKey("students"))
            {
                var students = (string)query["students"];
                Students.LoadAfterEdit(students);
            }
        }
    }
}
