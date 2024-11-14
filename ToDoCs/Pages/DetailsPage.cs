using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.ViewModels;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ToDoCs.Pages;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
    public DetailsPage(DetailsViewModel detailsViewModel) : base(detailsViewModel, "Details Page")
    {
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
                        new Entry()
                            .Placeholder("Edit title")
                            .FontSize(24)
                            .Bind(Entry.TextProperty, nameof(ViewModel.EditedTitle), BindingMode.TwoWay),

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
