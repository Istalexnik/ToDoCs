using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCs.Models;

namespace ToDoCs.ViewModels;
partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    string _newToDoText = string.Empty;
    [ObservableProperty]
    private bool isCompleted;
    public ObservableCollection<ToDoItem> ToDoItems { get; }

    public MainViewModel()
    {
        ToDoItems = new ObservableCollection<ToDoItem>();
    }

    [RelayCommand]
    private void AddToDo()
    {
        if (string.IsNullOrWhiteSpace(NewToDoText))
            return;

        ToDoItems.Add(new ToDoItem { Title = NewToDoText, IsCompleted = false });
        NewToDoText = string.Empty;
    }

    [RelayCommand]
    private void ToggleCompletion(ToDoItem item)
    {
        if (item != null)
        {
            item.IsCompleted = !item.IsCompleted;
        }
    }
}
