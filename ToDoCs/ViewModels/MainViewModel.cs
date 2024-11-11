using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic; // Import for Stack
using System.Windows.Input;
using ToDoCs.Models;
using ToDoCs.Pages;
using System.Diagnostics;

namespace ToDoCs.ViewModels;

partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string newToDoText = string.Empty;

    public ObservableCollection<ToDoItem> ToDoItems { get; }

    // Stack to track deleted items and their indices for multiple undo actions
    private Stack<(ToDoItem item, int index)> _deletedItemsStack;

    public MainViewModel()
    {
        ToDoItems = new ObservableCollection<ToDoItem>();
        _deletedItemsStack = new Stack<(ToDoItem, int)>();
        Debug.WriteLine($"{ToDoItems.Count} - MainViewModel constructor called");
    }

    [RelayCommand]
    private void AddToDo()
    {
        if (string.IsNullOrWhiteSpace(NewToDoText))
            return;

        ToDoItems.Add(new ToDoItem { Title = NewToDoText });
        NewToDoText = string.Empty;
    }

    [RelayCommand]
    private void DeleteTask(ToDoItem? item)
    {
        if (item != null && ToDoItems.Contains(item))
        {
            // Store the deleted item and its index on the stack
            int itemIndex = ToDoItems.IndexOf(item);
            _deletedItemsStack.Push((item, itemIndex));

            // Remove the item from the collection
            ToDoItems.Remove(item);

            // Show the Snackbar with an "Undo" action
            var snackbar = Snackbar.Make("Item deleted", () =>
            {
                if (_deletedItemsStack.Count > 0)
                {
                    // Retrieve the last deleted item from the stack and reinsert it
                    var (lastDeletedItem, lastDeletedIndex) = _deletedItemsStack.Pop();
                    ToDoItems.Insert(lastDeletedIndex, lastDeletedItem);
                }
            }, "Undo", TimeSpan.FromSeconds(3));

            snackbar.Show();
        }
    }

    [RelayCommand]
    public async Task OpenDetails(ToDoItem item)
    {
        if (item != null)
        {
            var navigationParameters = new Dictionary<string, object> { { "SelectedItem", item } };
            Debug.WriteLine($"Navigating to details with item: {item.Title}");
            await Shell.Current.GoToAsync("details", navigationParameters);
        }
    }




}
