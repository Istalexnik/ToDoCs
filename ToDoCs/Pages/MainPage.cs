using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ToDoCs.Models;
using ToDoCs.Pages;
using ToDoCs.ViewModels;

namespace ToDoCs;

class MainPage : BaseContentPage<MainViewModel>
{
    public MainPage(MainViewModel mainViewModel) : base(mainViewModel, "Main Page")
    {
        Content = new ScrollView
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
                        .Bind(Entry.TextProperty, nameof(mainViewModel.NewToDoText))
                        .TextColor(Colors.Green),

                    new Button()
                        .Text("Add ToDo")
                        .FontSize(24)
                        .BindCommand(nameof(mainViewModel.AddToDoCommand))
                        .TextColor(Colors.Green),

                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var titleLabel = new Label()
                                .Font(size: 18)
                                .TextColor(Colors.Black)
                                .Bind(Label.TextProperty, nameof(ToDoItem.Title));

                            var deleteSwipeItem = new SwipeItem
                            {
                                Text = "Delete",
                                BackgroundColor = Color.FromArgb("#f44336"),
                                IconImageSource = "delete_icon.png"
                            };

                            // Bind DeleteTaskCommand directly to Delete SwipeItem
                            deleteSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding(nameof(mainViewModel.DeleteTaskCommand), source: this.BindingContext));
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


                            swipeView.Content.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(async () =>
                                {
                                    var tappedItem = (ToDoItem)swipeView.BindingContext;
                                    if (tappedItem != null)
                                    {
                                        await mainViewModel.OpenDetails(tappedItem);
                                    }
                                }),
                                CommandParameter = new Binding(".")
                            });

                            return swipeView;
                        })
                    }
                    .Bind(CollectionView.ItemsSourceProperty, nameof(mainViewModel.ToDoItems))
                }
            }
        };
    }
}
