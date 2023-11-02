using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace pract8;

public partial class ListView : ContentPage
{
    private List<Student> students;
    public ListView()
	{
		InitializeComponent();
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            students = JsonConvert.DeserializeObject<List<Student>>(json);
            StudentsListView.ItemsSource = students;
        }
       
    }

    private void Load_Clicked(object sender, EventArgs e)
    {
        Refresh();
    }

    private void Remove_Clicked(object sender, EventArgs e)
    {
        if (StudentsListView.SelectedItem != null)
        {
            students.Remove(StudentsListView.SelectedItem as Student);
        }
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.json");
        string json = JsonConvert.SerializeObject(students);
        File.WriteAllText(filePath, json);
        Refresh();
    }

    private void Refresh()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            students = JsonConvert.DeserializeObject<List<Student>>(json);
            StudentsListView.ItemsSource = students;
        }
    }

    private void Edit_Clicked(object sender, EventArgs e)
    {
       
        if (StudentsListView.SelectedItem != null)
        {
            MainPage mainPage = new MainPage();
            mainPage.Edit(StudentsListView.SelectedItem as Student, students.IndexOf(StudentsListView.SelectedItem as Student));
            Navigation.PushAsync(mainPage);
        }
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        Refresh();
    }
}
