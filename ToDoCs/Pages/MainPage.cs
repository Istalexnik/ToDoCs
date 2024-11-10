using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.Models;
using ToDoCs.Pages;
using ToDoCs.ViewModels;

namespace ToDoCs;

class MainPage : BaseContentPage<MainViewModel>
{
    public MainPage(MainViewModel mainViewModel) : base(mainViewModel, "My Main Page")
    {
        Content = new ScrollView // Wrap the entire content in a ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(20),
                Spacing = 10,
                Children =
                {
                    new Label()
                        .Text("To-Do List")
                        .Font(size: 32)
                        .CenterHorizontal(),

                    new Entry()
                        .Placeholder("Enter new todo")
                        .FontSize(24)
                        .Bind(Entry.TextProperty, nameof(MainViewModel.NewToDoText))
                        .TextColor(Colors.Green),

                    new Button()
                        .Text("Add ToDo")
                        .FontSize(24)
                        .BindCommand(nameof(MainViewModel.AddToDoCommand))
                        .TextColor(Colors.Green),

                    new CollectionView
                    {

                        ItemTemplate = new DataTemplate(() =>
                        {
                            var titleLabel = new Label()
                                .Font(size: 18)
                                .TextColor(Colors.Black)
                                .Bind(Label.TextProperty, "Title");

                            var deleteSwipeItem = new SwipeItem
                            {
                                Text = "Delete",
                                BackgroundColor = Color.FromArgb("#f44336"),
                                IconImageSource = "delete_icon.png"
                            };
                            deleteSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding(nameof(MainViewModel.DeleteTaskCommand), source: this.BindingContext));
                            deleteSwipeItem.SetBinding(SwipeItem.CommandParameterProperty, new Binding("."));

                            var swipeView = new SwipeView
                            {
                                Threshold = 100,
                                RightItems = new SwipeItems { deleteSwipeItem }.Invoke(s => s.Mode = SwipeMode.Execute),
                                Content = new Frame
                                {
                                    Padding = 15,
                                    Margin = 10,
                                    CornerRadius = 8,
                                    BackgroundColor = Colors.White,
                                    Content = titleLabel
                                }
                            };

                            return swipeView;
                        })
                    }
                    .Bind(CollectionView.ItemsSourceProperty, nameof(MainViewModel.ToDoItems))
                }
            }
        };
    }
}
