
using Newtonsoft.Json;
using System.Threading;

namespace pract8
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GenderPicker.ItemsSource = genderList;
        }

        FileResult myPhoto;
        bool isEdit = false;
        int indexOfStudent = -1;

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (isEdit)
            {

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.json");
                string updatejson = File.ReadAllText(filePath);
                List<Student> existingStudents = new List<Student>();
                existingStudents = JsonConvert.DeserializeObject<List<Student>>(updatejson);
                Student editedStudent = existingStudents[indexOfStudent];
               
                //Student editedStudent = existingStudents[existingStudents.IndexOf(student)];
                editedStudent.FullName = FullNameEntry.Text;
                editedStudent.Gender = GenderPicker.SelectedItem.ToString();
                editedStudent.Age = Convert.ToInt32(AgeEntry.Text);
                if (myPhoto != null)
                {
                    editedStudent.PhotoPath = myPhoto.FullPath;// Укажите путь к фото
                }
                editedStudent.NeedsDormitory = NeedsDormitorySwitch.IsToggled;
                editedStudent.IsMonitor = IsMonitorSwitch.IsToggled;
                editedStudent.AvgRate = stepperAvgRate.Value;
                string updatedJson = JsonConvert.SerializeObject(existingStudents);
                File.WriteAllText(filePath, updatedJson);
                isEdit = false;
            }
            else
            {
                // Создайте нового студента
                Student newStudent = new Student
                {
                    FullName = FullNameEntry.Text,
                    Gender = GenderPicker.SelectedItem.ToString(),
                    Age = Convert.ToInt32(AgeEntry.Text),
                    PhotoPath = myPhoto.FullPath, // Укажите путь к фото
                    NeedsDormitory = NeedsDormitorySwitch.IsToggled,
                    IsMonitor = IsMonitorSwitch.IsToggled,
                    AvgRate = stepperAvgRate.Value
                };

                // Проверьте, существует ли файл JSON
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.json");
                List<Student> existingStudents = new List<Student>();

                if (File.Exists(filePath))
                {
                    // Если файл существует, загрузите существующие данные
                    string json = File.ReadAllText(filePath);
                    existingStudents = JsonConvert.DeserializeObject<List<Student>>(json);
                }

                // Добавьте нового студента к существующим данным
                existingStudents.Add(newStudent);

                // Сохраните обновленные данные в файл JSON
                string updatedJson = JsonConvert.SerializeObject(existingStudents);
                File.WriteAllText(filePath, updatedJson);

                // Очистите поля ввода
                FullNameEntry.Text = string.Empty;
                AgeEntry.Text = string.Empty;
                GenderPicker.SelectedIndex = -1;
                NeedsDormitorySwitch.IsToggled = false;
                IsMonitorSwitch.IsToggled = false;
                stepperAvgRate.Value = 0;
                myPhoto = null;
                UserImage = null;
            }

        }

        private async Task SaveStudentWithPhotoAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                myPhoto = await MediaPicker.Default.PickPhotoAsync();
                if (myPhoto != null)
                {
                    UserImage.Source = myPhoto.FullPath;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("OOPS", "Your device isn't supported", "OK");
            }
        }

        private async void LoadPhoto_Click(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                myPhoto = await MediaPicker.Default.PickPhotoAsync();
                if (myPhoto != null)
                {
                    UserImage.Source = myPhoto.FullPath;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("OOPS", "Your device isn't supported", "OK");
            }
        }

        public void Edit(Student student, int index)
        {

            FullNameEntry.Text = student.FullName;
            AgeEntry.Text = Convert.ToString(student.Age);
            NeedsDormitorySwitch.IsToggled = student.NeedsDormitory;
            GenderPicker.SelectedIndex = genderList.IndexOf(student.Gender);
            IsMonitorSwitch.IsToggled = student.IsMonitor;
            stepperAvgRate.Value = student.AvgRate;
            UserImage.Source = student.PhotoPath;
            isEdit = true;
            indexOfStudent = index;
        }



        List<string> genderList = new List<string>
            {
                "Мужской",
                "Женский"
            };
    }

    public class Student
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string PhotoPath { get; set; }
        public bool NeedsDormitory { get; set; }
        public bool IsMonitor { get; set; }
        public double AvgRate { get; set; }
    }
}


