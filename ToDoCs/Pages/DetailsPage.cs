using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.ViewModels;

namespace ToDoCs.Pages;

class DetailsPage : BaseContentPage<DetailsViewModel>
{
    private Entry _titleEntry; // Reference to the Entry for setting focus

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
                _titleEntry, // Use the named Entry for focus
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait a short moment to ensure the page is fully rendered
        await Task.Delay(100);

        // Set focus to the Entry
        _titleEntry.Focus();
        _titleEntry.CursorPosition = _titleEntry.Text?.Length ?? 0;
    }

}
