using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.ViewModels;

namespace ToDoCs.Pages;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
    private Entry _titleEntry;

    public DetailsPage(DetailsViewModel detailsViewModel) : base(detailsViewModel, "Details Page")
    {
        _titleEntry = new Entry()
            .Placeholder("Edit title")
            .FontSize(24)
            .Bind(Entry.TextProperty, nameof(detailsViewModel.EditedTitle), BindingMode.TwoWay);

        Content = new StackLayout
        {
            Padding = new Thickness(20),
            Children =
            {
                _titleEntry,

                // Corrected binding paths
                CreateFormattedLabel("Created on: ", "SelectedItem.CreatedDate"),
                CreateFormattedLabel("Edited on: ", "SelectedItem.EditedDate"),

                new Button()
                    .Text("Save")
                    .FontSize(24)
                    .BindCommand(nameof(detailsViewModel.SaveCommand)),

                new Button()
                    .Text("Delete")
                    .FontSize(24)
                    .TextColor(Colors.Red)
                    .BindCommand(nameof(detailsViewModel.DeleteCommand))
            }
        };
    }

    private Label CreateFormattedLabel(string prefixText, string dateBindingPath) =>
        new Label
        {
            FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = prefixText, TextColor = Colors.Green },
                    new Span
                    {
                        TextColor = Colors.Red
                    }
                    .Bind<Span, DateTime?, string>(
                        Span.TextProperty,
                        dateBindingPath,
                        convert: (DateTime? date) => date.HasValue ? date.Value.ToString("yyyy-MM-dd HH:mm") : "")
                }
            }
        };

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(100);
        _titleEntry.Focus();
        _titleEntry.CursorPosition = _titleEntry.Text?.Length ?? 0;
    }
}
