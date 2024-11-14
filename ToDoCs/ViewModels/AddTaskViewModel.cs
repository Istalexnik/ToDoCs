using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCs.Models;
using ToDoCs.Services;

namespace ToDoCs.ViewModels;
 partial class AddTaskViewModel : BaseViewModel
{
    private readonly ToDoService _toDoService;

    [ObservableProperty]
    private string newToDoText = string.Empty;

    public AddTaskViewModel(ToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    public async Task SaveTaskAsync()
    {
        if (!string.IsNullOrWhiteSpace(NewToDoText))
        {
            var newTask = new ToDoItem
            {
                Title = NewToDoText               
            };

            await _toDoService.AddToDoItemAsync(newTask);
        }
    }

}
