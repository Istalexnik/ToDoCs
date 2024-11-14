using CommunityToolkit.Maui.Markup;
using ToDoCs.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ToDoCs.Pages;

class AddTaskPage : BaseContentPage<AddTaskViewModel>
{
    public AddTaskPage(AddTaskViewModel addTaskViewModel) : base(addTaskViewModel, "Add Task")
    {
        Content = new Grid
        {
            ColumnDefinitions = Columns.Define(Star, Auto), // Ensure correct semicolon
            ColumnSpacing = 10,
            Children =
            {
                new Frame
                {
                    BackgroundColor = Colors.White,
                    CornerRadius = 8,
                    Padding = 0,
                    HasShadow = false,
                    Content = new Entry
                    {
                        Placeholder = "Enter new todo",
                        PlaceholderColor = Color.FromArgb("#DDDDDD"),
                        FontSize = 18,
                        TextColor = Colors.Black,
                        BackgroundColor = Colors.Transparent
                    }
                    .Bind(Entry.TextProperty, nameof(ViewModel.NewToDoText))
                }
                .Column(0)
            }
        };
    }


    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await ViewModel.SaveTaskAsync(); // Save the task to the database
    }

}
