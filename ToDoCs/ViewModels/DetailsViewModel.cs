using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoCs.Models;

namespace ToDoCs.ViewModels;

[QueryProperty(nameof(SelectedItem), "SelectedItem")]
partial class DetailsViewModel : BaseViewModel
{
    private readonly MainViewModel _mainViewModel;

    // Stack to track the deleted item for undo purposes
    private Stack<(ToDoItem item, int index)> _deletedItemStack;

    public DetailsViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _deletedItemStack = new Stack<(ToDoItem, int)>();
        Debug.WriteLine("DetailsViewModel created with mainViewModel");
    }

    [ObservableProperty]
    private string editedTitle = string.Empty;

    [ObservableProperty]
    private ToDoItem? selectedItem;

    partial void OnSelectedItemChanged(ToDoItem? value)
    {
        if (value != null)
        {
            EditedTitle = value.Title; // Preload EditedTitle with the current title of SelectedItem
            Debug.WriteLine($"{value.Title} - SelectedItem assigned to DetailsViewModel");
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (SelectedItem != null)
        {
            var itemIndex = _mainViewModel.ToDoItems.IndexOf(SelectedItem);

            if (itemIndex >= 0)
            {
                // Remove and re-add the item with the updated title
                var updatedItem = new ToDoItem { Title = EditedTitle };
                _mainViewModel.ToDoItems[itemIndex] = updatedItem;

                Debug.WriteLine($"Updated item at index {itemIndex} with new title: {EditedTitle}");
            }
            else
            {
                Debug.WriteLine("Selected item was not found in ToDoItems");
            }
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedItem != null)
        {
            var itemIndex = _mainViewModel.ToDoItems.IndexOf(SelectedItem);

            if (itemIndex >= 0)
            {
                // Store the deleted item and its index for undo
                _deletedItemStack.Push((SelectedItem, itemIndex));
                _mainViewModel.ToDoItems.RemoveAt(itemIndex);

                // Show the Snackbar with an "Undo" action
                var snackbar = Snackbar.Make("Item deleted", () =>
                {
                    if (_deletedItemStack.Count > 0)
                    {
                        // Retrieve the last deleted item and reinsert it
                        var (lastDeletedItem, lastDeletedIndex) = _deletedItemStack.Pop();
                        _mainViewModel.ToDoItems.Insert(lastDeletedIndex, lastDeletedItem);
                    }
                }, "Undo", TimeSpan.FromSeconds(3));

                snackbar.Show();
                Debug.WriteLine($"Deleted item: {SelectedItem.Title}");
            }
            else
            {
                Debug.WriteLine("Selected item was not found in ToDoItems");
            }
        }
        else
        {
            Debug.WriteLine("SelectedItem is null in Delete command");
        }

        await Shell.Current.GoToAsync("..");
    }
}
