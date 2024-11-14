using AndroidX.Lifecycle;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoCs.Helpers;
using ToDoCs.Models;
using ToDoCs.Pages;
using ToDoCs.Services;

namespace ToDoCs.ViewModels;

 partial class MainViewModel : BaseViewModel
{
    private readonly ToDoService _toDoService;

    public ToDoService ToDoService => _toDoService; // Expose ToDoService here



    public ObservableCollection<ToDoItem> ToDoItems { get; set; }

    private Stack<(ToDoItem item, int index)> _deletedItemsStack;

    public MainViewModel(ToDoService toDoService)
    {
        _toDoService = toDoService;
        ToDoItems = new ObservableCollection<ToDoItem>();
        _deletedItemsStack = new Stack<(ToDoItem, int)>();

        LoadToDoItems();

    }

    public async void LoadToDoItems()
    {
        // Clear the existing items
        ToDoItems.Clear();

        // Fetch the updated list from the database
        var items = await ToDoService.GetAllToDoItemsAsync();

        // Add the items back into the ObservableCollection
        foreach (var item in items)
        {
            ToDoItems.Add(item);
        }

        // Notify the UI that the collection has been updated
        OnPropertyChanged(nameof(ToDoItems));
    }



    [RelayCommand]
    private async Task AddToDo()
    {
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }


    [RelayCommand]
    private async Task DeleteTask(ToDoItem? item)
    {
        if (item != null && ToDoItems.Contains(item))
        {
            int itemIndex = ToDoItems.IndexOf(item);
            _deletedItemsStack.Push((item, itemIndex));

            await _toDoService.DeleteToDoItemAsync(item);
            ToDoItems.Remove(item);

            var snackbar = Snackbar.Make("Item deleted", async () =>
            {
                if (_deletedItemsStack.Count > 0)
                {
                    var (lastDeletedItem, lastDeletedIndex) = _deletedItemsStack.Pop();
                    await _toDoService.AddToDoItemAsync(lastDeletedItem);
                    ToDoItems.Insert(lastDeletedIndex, lastDeletedItem);
                }
            }, "Undo", TimeSpan.FromSeconds(3));

            await snackbar.Show();
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
