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

    [ObservableProperty]
    private string editedTitle = string.Empty;

    public DetailsViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        Debug.WriteLine("DetailsViewModel created with mainViewModel");
    }

    // Observable property for SelectedItem with additional debug logic in the setter
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
                var updatedItem = new ToDoItem { Title = EditedTitle }; // Create a new item with the edited title
                _mainViewModel.ToDoItems[itemIndex] = updatedItem; // Replace the item to force UI update

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
            var itemToDelete = _mainViewModel.ToDoItems.FirstOrDefault(i => i == SelectedItem);
            if (itemToDelete != null)
            {
                _mainViewModel.ToDoItems.Remove(itemToDelete);
                Debug.WriteLine($"Deleted item: {itemToDelete.Title}");
            }
            else
            {
                Debug.WriteLine("Item to delete was not found in ToDoItems");
            }
        }
        else
        {
            Debug.WriteLine("SelectedItem is null in Delete command");
        }

        await Shell.Current.GoToAsync(".."); // Navigate back after deletion
    }
}
