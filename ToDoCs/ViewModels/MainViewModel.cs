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

    private void DeleteTask(ToDoItem item)
    {
        if (item != null && ToDoItems.Contains(item))
        {
            System.Diagnostics.Debug.WriteLine($"Deleting item: {item.Title}");
            ToDoItems.Remove(item);
        }
    }
}
