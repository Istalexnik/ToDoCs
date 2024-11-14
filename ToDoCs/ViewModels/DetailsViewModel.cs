using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoCs.Helpers;
using ToDoCs.Models;

namespace ToDoCs.ViewModels;
[QueryProperty(nameof(SelectedItem), "SelectedItem")]
partial class DetailsViewModel : BaseViewModel
{
    private readonly MainViewModel _mainViewModel;

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

    public string CreatedDateText => SelectedItem?.CreatedDate.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;
    public string EditedDateText => SelectedItem?.EditedDate.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;

    partial void OnSelectedItemChanged(ToDoItem? value)
    {
        if (value != null)
        {
            EditedTitle = value.Title; // Preload EditedTitle with the current title of SelectedItem
            OnPropertyChanged(nameof(CreatedDateText));
            OnPropertyChanged(nameof(EditedDateText));
            Debug.WriteLine($"{value.Title} - SelectedItem assigned to DetailsViewModel");
        }
    }



    [RelayCommand]
    private async Task Save()
    {
        if (SelectedItem != null)
        {
            // Update the selected item's properties
            SelectedItem.Title = EditedTitle;
            SelectedItem.EditedDate = DateTime.Now;

            // Notify that EditedDateText has changed
            OnPropertyChanged(nameof(EditedDateText));

            // Save the updated item to the database
            await _mainViewModel.ToDoService.UpdateToDoItemAsync(SelectedItem);

            Debug.WriteLine($"Updated item with new title: {EditedTitle}");
        }

        // Navigate back to MainPage
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
                // Store the deleted item for undo functionality
                _deletedItemStack.Push((SelectedItem, itemIndex));
                _mainViewModel.ToDoItems.RemoveAt(itemIndex);

                // Delete the item from the database
                await _mainViewModel.ToDoService.DeleteToDoItemAsync(SelectedItem);

                if (DeviceInfo.Platform != DevicePlatform.WinUI)
                {
                    // Show the Snackbar with an "Undo" action
                    var snackbar = Snackbar.Make("Item deleted", async () =>
                {
                    if (_deletedItemStack.Count > 0)
                    {
                        // Retrieve the last deleted item and reinsert it
                        var (lastDeletedItem, lastDeletedIndex) = _deletedItemStack.Pop();
                        await _mainViewModel.ToDoService.AddToDoItemAsync(lastDeletedItem);
                        _mainViewModel.ToDoItems.Insert(lastDeletedIndex, lastDeletedItem);
                    }
                }, "Undo", TimeSpan.FromSeconds(3));

                    await snackbar.Show();
                }
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
