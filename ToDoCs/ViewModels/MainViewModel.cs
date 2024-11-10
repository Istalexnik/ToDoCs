using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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

    private ToDoItem? _lastDeletedItem; // Nullable to address CS8618 and CS8625 warnings
    private int _lastDeletedItemIndex;

    public ICommand DeleteTaskCommand { get; }

    public MainViewModel()
    {
        ToDoItems = new ObservableCollection<ToDoItem>();

        // Explicitly define DeleteTaskCommand
        DeleteTaskCommand = new RelayCommand<ToDoItem?>(DeleteTask);
    }

    [RelayCommand]
    private void AddToDo()
    {
        if (string.IsNullOrWhiteSpace(NewToDoText))
            return;

        ToDoItems.Add(new ToDoItem { Title = NewToDoText });
        NewToDoText = string.Empty;
    }

    // Remove 'async' since there is no await call inside this method
    private void DeleteTask(ToDoItem? item)
    {
        if (item != null && ToDoItems.Contains(item))
        {
            // Store the deleted item and its index
            _lastDeletedItem = item;
            _lastDeletedItemIndex = ToDoItems.IndexOf(item);

            // Remove the item from the collection
            ToDoItems.Remove(item);

            // Show the Snackbar with an "Undo" action
            var snackbar = Snackbar.Make("Item deleted", () =>
            {
                // Restore the item if "Undo" is clicked
                ToDoItems.Insert(_lastDeletedItemIndex, _lastDeletedItem);
                _lastDeletedItem = null; // Clear after undo
            }, "Undo", TimeSpan.FromSeconds(3));

            snackbar.Show();
        }
    }

}
