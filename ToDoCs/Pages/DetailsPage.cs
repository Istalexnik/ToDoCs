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

        Content = new VerticalStackLayout
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

                // Use Grid instead of HorizontalStackLayout
                new Grid
                {
                    ColumnDefinitions = Columns.Define(Star, Star),
                    ColumnSpacing = 20,
                    Children =
                    {
                        new Button()
                            .Text("Save")
                            .FontSize(24)
                            .BindCommand(nameof(ViewModel.SaveCommand))
                            .BackgroundColor(Colors.Green)
                            .TextColor(Colors.White)
                            .Column(0),

                        new Button()
                            .Text("Delete")
                            .FontSize(24)
                            .TextColor(Colors.White)
                            .BackgroundColor(Colors.Red)
                            .BindCommand(nameof(ViewModel.DeleteCommand))
                            .Column(1)
                    }
                }
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
