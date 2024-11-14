using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using ToDoCs.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ToDoCs.Pages;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
    private readonly Editor _editor;

    public DetailsPage(DetailsViewModel detailsViewModel) : base(detailsViewModel, "Details Page")
    {
        // Initialize the Editor
        _editor = new Editor
        {
            Placeholder = "Edit title",
            FontSize = 20,
            TextColor = Colors.Black,
            BackgroundColor = Colors.Transparent,
            AutoSize = EditorAutoSizeOption.TextChanges
        }
        .Bind(Editor.TextProperty, nameof(ViewModel.EditedTitle), BindingMode.TwoWay);

        Content = new Grid
        {
            RowDefinitions = Rows.Define(Star, Auto),
            ColumnDefinitions = Columns.Define(Star),

            Children =
            {
                // Frame with rounded corners for Editor
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

                // Floating Save button (positioned middle-right)
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40,
                    BackgroundColor = Color.FromRgba("#AAAAAA"),
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "✔",
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    }
                }
                .RowSpan(2)
                .ColumnSpan(2)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .LayoutBounds(1, 0.5) // Middle-right
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync(null))
                })),

                // Floating Delete button (positioned middle-left)
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40,
                    BackgroundColor = Color.FromRgba("#AAAAAA"),
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "✖",
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    }
                }
                .RowSpan(2)
                .ColumnSpan(2)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .LayoutBounds(0, 0.5) // Middle-left
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => await ViewModel.DeleteCommand.ExecuteAsync(null))
                }))
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100);
        _editor.Focus();
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
