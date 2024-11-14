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
            FontSize = 24,
            TextColor = Colors.Black,
            BackgroundColor = Colors.Transparent, // Make Editor background transparent
            AutoSize = EditorAutoSizeOption.TextChanges // Enable automatic resizing
        }
        .Bind(Editor.TextProperty, nameof(ViewModel.EditedTitle), BindingMode.TwoWay);

        Content = new Grid
        {
            Children =
            {
                // Main content
                new VerticalStackLayout
                {
                    Padding = new Thickness(20),
                    Spacing = 20,
                    BackgroundColor = Color.FromRgba("#999999"),
                    Children =
                    {
                        // Wrap Editor in a Frame to apply rounded corners
                        new Frame
                        {
                            CornerRadius = 8,
                            BorderColor = Colors.Gray,
                            Padding = 0, // Remove padding to let Editor fill the Frame
                            BackgroundColor = Colors.White,
                            Content = _editor // Set the Editor as content
                        },

                        CreateDateInfo(),
                    }
                },

                // Floating circular "Save" button on the middle-right
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
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 10, 0) // Right margin for spacing
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await ViewModel.SaveCommand.ExecuteAsync(null); // Execute the save command
                    })
                })),

                // Floating circular "Delete" button on the middle-left
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
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 0, 0) // Left margin for spacing
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await ViewModel.DeleteCommand.ExecuteAsync(null); // Execute the delete command
                    })
                }))
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
            RowDefinitions = Rows.Define(
                (Row.Created, Auto),
                (Row.Edited, Auto)
            ),
            ColumnDefinitions = Columns.Define(
                (Column.Label, Auto),
                (Column.Value, Star)
            ),
            Children =
            {
                new Label { Text = "Created on: ", FontAttributes = FontAttributes.Bold }
                    .Row(Row.Created).Column(Column.Label),
                new Label()
                    .Bind(Label.TextProperty, nameof(ViewModel.CreatedDateText))
                    .Row(Row.Created).Column(Column.Value),

                new Label { Text = "Edited on: ", FontAttributes = FontAttributes.Bold }
                    .Row(Row.Edited).Column(Column.Label),
                new Label()
                    .Bind(Label.TextProperty, nameof(ViewModel.EditedDateText))
                    .Row(Row.Edited).Column(Column.Value),
            }
        };
    }

    // Enums must be declared at class scope, not inside methods
    enum Row { Created, Edited }
    enum Column { Label, Value }
}
