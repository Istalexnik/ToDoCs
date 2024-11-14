using CommunityToolkit.Maui.Markup;
using ToDoCs.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ToDoCs.Pages;

class AddTaskPage : BaseContentPage<AddTaskViewModel>
{
    private readonly Editor _taskEditor;

    public AddTaskPage(AddTaskViewModel addTaskViewModel) : base(addTaskViewModel, "Add Task")
    {
        // Initialize the Editor and store it in the _taskEditor field
        _taskEditor = new Editor
        {
            Placeholder = "Enter new todo",
            PlaceholderColor = Color.FromArgb("#DDDDDD"),
            FontSize = 18,
            TextColor = Colors.Black,
            BackgroundColor = Colors.Transparent,
            AutoSize = EditorAutoSizeOption.TextChanges
        }
        .Bind(Editor.TextProperty, nameof(ViewModel.NewToDoText));

        Content = new Grid
        {
            Padding = 0, // Remove any padding around the grid
            Children =
            {
                new Frame
                {
                    BackgroundColor = Colors.White,
                    Padding = new Thickness(10), // Adjust if you want space around the editor
                    HasShadow = false,
                    Content = _taskEditor
                }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100);
        _taskEditor.Focus(); // Set focus to the Editor when the page appears
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await ViewModel.SaveTaskAsync(); // Save the task to the database
    }
}
