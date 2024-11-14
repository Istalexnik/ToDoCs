using CommunityToolkit.Maui.Markup;
using ToDoCs.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ToDoCs.Pages;

class AddTaskPage : BaseContentPage<AddTaskViewModel>
{
    private readonly Editor _taskEditor;

    public AddTaskPage(AddTaskViewModel addTaskViewModel) : base(addTaskViewModel, "Add Task")
    {
        // Initialize the Editor and place it inside a Frame for rounded corners
        _taskEditor = new Editor
        {
            Placeholder = "Enter new task",
            PlaceholderColor = Color.FromArgb("#DDDDDD"),
            FontSize = 18,
            TextColor = Colors.Black,
            BackgroundColor = Colors.Transparent,
            AutoSize = EditorAutoSizeOption.TextChanges
        }
        .Bind(Editor.TextProperty, nameof(ViewModel.NewToDoText));

        // Define the main content and floating save button
        Content = new Grid
        {
            Children =
            {
                // Frame with rounded corners to hold the Editor
                new Frame
                {
                    CornerRadius = 8,
                    BorderColor = Colors.Gray,
                    Padding = new Thickness(10),
                    BackgroundColor = Colors.White,
                    HasShadow = false,
                    Content = _taskEditor
                },

                // Floating "Save" button positioned in the middle-right side of the screen
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40, // Make it circular
                    BackgroundColor = Color.FromRgba("#AAAAAA"), // Set color for the save button
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "V", // Save icon or check mark
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    },
                    HorizontalOptions = LayoutOptions.End, // Align to the right
                    VerticalOptions = LayoutOptions.Center, // Center vertically
                    Margin = new Thickness(0, 0, 10, 0) // Right margin for spacing
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await ViewModel.SaveTaskAsync();
                        await Shell.Current.GoToAsync(".."); // Navigate back after saving
                    })
                }))
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100); // Small delay to ensure UI is fully loaded
        _taskEditor.Focus(); // Set focus to the Editor when the page appears
    }
}
