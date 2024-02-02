using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AnsPi.ViewModels
{
    public class ClassViewModel : IQueryAttributable
    {
        public ObservableCollection<Models.Student> Students { get; private set; } = new();
        private ulong[] lastRolled = new ulong[3] { 0, 0, 0 };
        private uint luckyNumber = 0;
        private string selectedClass = "";

        public ICommand ChangeClassCommand { get; set; }
        public ICommand AddClassCommand { get; set; }
        public ICommand EditClassCommand { get; set; }
        public ICommand LuckyNumberCommand { get; set; }
        public ICommand RollCommand { get; set; }

        public ClassViewModel() {
            ChangeClassCommand = new AsyncRelayCommand(ChangeClass);
            AddClassCommand = new AsyncRelayCommand(AddClass);
            EditClassCommand = new AsyncRelayCommand(EditClass);
            LuckyNumberCommand = new AsyncRelayCommand(ChangeLuckyNumber);
            RollCommand = new AsyncRelayCommand(RollStudent);
        }

        private async Task<bool> CheckClass()
        {
            if(Students.Count == 0)
            {
                await Shell.Current.DisplayAlert("Message", "Please select a class first!", "OK");
                return false;
            }
            return true; 
        }

        private async Task ChangeClass()
        {
            string[] classes = Storage.GetClasses();
            string selected = await Shell.Current.DisplayActionSheet("Choose class", "Cancel", null, classes);
            if(string.IsNullOrEmpty(selected)) return;
            selectedClass = selected;

            Students.Clear();
            foreach(var student in Storage.GetStudents(selected))
            {
                Students.Add(student);
            }
        }

        private async Task AddClass()
        {
            string className = await Shell.Current.DisplayPromptAsync("Add Class", "Enter new class name: ");
            if(!string.IsNullOrWhiteSpace(className) && className.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
            {
                Storage.AddClass(className);
                await Shell.Current.GoToAsync(nameof(Views.EditClassPage));
            }
        }

        private async Task EditClass()
        {
            if (!await CheckClass()) return;
            Dictionary<string, object> input = new()
            {
                { "input", string.Join('\r', Students.Select(student => student.FullName)) }
            };
            await Shell.Current.GoToAsync(nameof(Views.EditClassPage), input);
        }

        private async Task ChangeLuckyNumber()
        {
            if (!await CheckClass()) return;

            string newLucky =
                await Shell.Current.DisplayPromptAsync("Lucky Number", $"Current Lucky Number is: {luckyNumber}", "OK", "Random");

            if(newLucky == null)
            {
                int numberIdx = new Random().Next(Students.Count);
                luckyNumber = Students[numberIdx].Number;
            }
            else if(newLucky.Length != 0)
            {
                luckyNumber = uint.TryParse(newLucky, out var lucky) ? lucky : luckyNumber;
            }
        }

        private async Task RollStudent()
        {
            if (!await CheckClass()) return;

            Trace.WriteLine(Students[0].GetHashCode());

            var valid = Students.Where
                (student => student.Number != luckyNumber &&
                student.Present && !lastRolled.Contains(student.GetDeterministicHashCode()));
            int chosen = new Random().Next(valid.Count());
            var chosenStudent = valid.ElementAt(chosen);

            await Shell.Current.DisplayAlert("Roll", $"{chosenStudent} has been selected for answer!", "OK");
            UpdateLastRolled(chosenStudent.GetDeterministicHashCode());
        }

        private void UpdateLastRolled(ulong inserted)
        {
            lastRolled[0] = lastRolled[1];
            lastRolled[1] = lastRolled[2];
            lastRolled[2] = inserted;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.ContainsKey("students"))
            {
                Students.Clear();
                var students = (IEnumerable<Models.Student>)query["students"];
                foreach(var student in students)
                {
                    Students.Add(student);
                }
            }
        }
    }
}
