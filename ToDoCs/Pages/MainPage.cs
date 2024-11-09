using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCs.Models;
using ToDoCs.Pages;
using ToDoCs.ViewModels;

namespace ToDoCs;
class MainPage : BaseContentPage<MainViewModel>
{
    public MainPage(MainViewModel mainViewModel) : base(mainViewModel, "My Main Page")
    {
        Content = new StackLayout()
        {
            Padding = new Thickness(20),
            Children =
            {
                new Label()
                .Text("To-Do List")
                .Font(size: 32)
                .CenterHorizontal(),

                new Entry()
                .Placeholder("Enter new todo")
                .Bind(Entry.TextProperty, nameof(MainViewModel.NewToDoText))
                .TextColor(Colors.Green),

                new Button()
                .Text("Add ToDo")
                .BindCommand(nameof(MainViewModel.AddToDoCommand))
                .TextColor(Colors.Blue),

                new ListView()
                {
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var titleLabel = new Label()
                        .Font(size: 18)
                        .TextColor(Colors.Black)
                        .Bind(Label.TextProperty, nameof(ToDoItem.Title));

                        var completedSwitch = new Switch()
                        .Bind(Switch.IsToggledProperty, nameof(ToDoItem.IsCompleted))
                        .Invoke(s => s.Toggled += (sender, args) =>
                        {
                             var viewModel = BindingContext;
                             var item = (ToDoItem)((Switch)sender!).BindingContext;  
                             viewModel.ToggleCompletionCommand.Execute(item);
                        });

                        return new ViewCell()
                        {
                            View = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = {titleLabel, completedSwitch }
                            }
                        };
                    })
                }
                .Bind(ListView.ItemsSourceProperty, nameof(MainViewModel.ToDoItems))
            }
        };
    }
}
