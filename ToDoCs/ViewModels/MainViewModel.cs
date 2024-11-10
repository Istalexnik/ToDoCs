using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoCs.Models;

namespace ToDoCs.ViewModels;

partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string newToDoText = string.Empty;

    public ObservableCollection<ToDoItem> ToDoItems { get; }

    private ToDoItem _lastDeletedItem; // Stores the last deleted item temporarily
    private int _lastDeletedItemIndex; // Stores the last deleted item index for restoration

    public ICommand DeleteTaskCommand { get; }

    public MainViewModel()
    {
        ToDoItems = new ObservableCollection<ToDoItem>();

        // Explicitly define DeleteTaskCommand
        DeleteTaskCommand = new RelayCommand<ToDoItem>(DeleteTask);
    }

    [RelayCommand]
    private void AddToDo()
    {
        if (string.IsNullOrWhiteSpace(NewToDoText))
            return;

        ToDoItems.Add(new ToDoItem { Title = NewToDoText });
        NewToDoText = string.Empty;
    }

    private async void DeleteTask(ToDoItem item)
    {
        if (item != null && ToDoItems.Contains(item))
        {
            // Store the deleted item and its index
            _lastDeletedItem = item;
            _lastDeletedItemIndex = ToDoItems.IndexOf(item);

            // Remove the item from the collection
            ToDoItems.Remove(item);

            // Show the Snackbar with an "Undo" action
            var snackbar = Snackbar.Make("Item deleted", async () =>
            {
                // Restore the item if "Undo" is clicked
                ToDoItems.Insert(_lastDeletedItemIndex, _lastDeletedItem);
                _lastDeletedItem = null; // Clear after undo
            }, "Undo", TimeSpan.FromSeconds(3));

            await snackbar.Show();
        }
    }
}
