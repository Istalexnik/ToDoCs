using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ToDoCs.Pages;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
    private readonly Editor _editor;

    public DetailsPage(DetailsViewModel detailsViewModel) : base(detailsViewModel, "Details Page")
    {
        // Initialize the Editor and store it in the _editor field
        _editor = new Editor
        {
            Placeholder = "Edit title",
            FontSize = 20,
            TextColor = Colors.Black,
            BackgroundColor = Colors.Transparent, // Transparent to inherit frame's background
            AutoSize = EditorAutoSizeOption.TextChanges // Enable automatic resizing
        }
        .Bind(Editor.TextProperty, nameof(ViewModel.EditedTitle), BindingMode.TwoWay);

        Content = new Grid
        {
            RowDefinitions = Rows.Define(Star, Auto),
            ColumnDefinitions = Columns.Define(Star),

            Children =
            {
                // Main content with Editor wrapped in a Frame
                new Frame
                {
                    CornerRadius = 12,
                    BorderColor = Colors.Gray,
                    BackgroundColor = Colors.White,
                    Padding = new Thickness(20),
                    Content = _editor
                }
                .Row(0),

                // Date Info section
                CreateDateInfo()
                .Row(1),

                // Floating Save button on the right side, centered vertically
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40, // Make it circular
                    BackgroundColor = Color.FromRgba("#AAAAAA"),
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "✔",
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    },
                    HorizontalOptions = LayoutOptions.End, // Align to the right
                    VerticalOptions = LayoutOptions.Center, // Center vertically
                    Margin = new Thickness(0, 0, 20, 0) // Margin to position towards the right edge
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await ViewModel.SaveCommand.ExecuteAsync(null); // Execute the save command
                    })
                }))
                .Row(0)
                .Column(0),

                // Floating Delete button on the left side, centered vertically
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40, // Make it circular
                    BackgroundColor = Color.FromRgba("#AAAAAA"),
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "✖",
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    },
                    HorizontalOptions = LayoutOptions.Start, // Align to the left
                    VerticalOptions = LayoutOptions.Center, // Center vertically
                    Margin = new Thickness(20, 0, 0, 0) // Margin to position towards the left edge
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await ViewModel.DeleteCommand.ExecuteAsync(null); // Execute the delete command
                    })
                }))
                .Row(0)
                .Column(0),
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100); // Small delay to ensure UI is fully loaded

        // Set focus to the Editor
        _editor.Focus();

        // Optionally add a space or newline at the end
        if (!string.IsNullOrEmpty(_editor.Text))
        {
            _editor.Text += " "; // Add a space (or "\n" for a new line)
        }

        // Move the cursor to the end of the existing text
        _editor.CursorPosition = _editor.Text.Length;
    }

    private View CreateDateInfo()
    {
        return new Grid
        {
            RowDefinitions = Rows.Define(Auto, Auto),
            ColumnDefinitions = Columns.Define(Auto, Star),

            Children =
            {
                new Label { Text = "Created on: ", FontAttributes = FontAttributes.Bold }
                    .Row(0).Column(0),
                new Label()
                    .Bind(Label.TextProperty, nameof(ViewModel.CreatedDateText))
                    .Row(0).Column(1),

                new Label { Text = "Edited on: ", FontAttributes = FontAttributes.Bold }
                    .Row(1).Column(0),
                new Label()
                    .Bind(Label.TextProperty, nameof(ViewModel.EditedDateText))
                    .Row(1).Column(1)
            }
        };
    }
}
