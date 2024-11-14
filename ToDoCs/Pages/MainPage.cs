using CommunityToolkit.Maui.Markup;
using ToDoCs.Models;
using ToDoCs.Pages;
using ToDoCs.ViewModels;

namespace ToDoCs;

class MainPage : BaseContentPage<MainViewModel>
{
    public MainPage(MainViewModel mainViewModel) : base(mainViewModel, "Main Page")
    {
        Content = new Grid
        {
            Children =
            {
                // Main content in a ScrollView
                new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = new Thickness(20),
                        Spacing = 20,
                        BackgroundColor = Color.FromRgba("#999999"),
                        Children =
                        {
                            new Label()
                                .Text("Notes")
                                .Font(size: 32)
                                .CenterHorizontal(),

                            // CollectionView for the to-do items
                            new CollectionView
                            {
                                ItemsLayout = LinearItemsLayout.Vertical,
                                ItemTemplate = ToDoItemTemplate()
                            }
                            .Bind(CollectionView.ItemsSourceProperty, nameof(ViewModel.ToDoItems))
                        }
                    }
                },

                // Floating action button positioned in the middle-right side of the screen
                new Frame
                {
                    WidthRequest = 80,
                    HeightRequest = 80,
                    CornerRadius = 40, // Make it circular
                    BackgroundColor = Color.FromRgba("#AAAAAA"), // Adjust to your preferred color
                    Padding = 0,
                    Content = new Label
                    {
                        Text = "+",
                        FontSize = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    },
                    HorizontalOptions = LayoutOptions.End, // Align to the right
                    VerticalOptions = LayoutOptions.Center, // Center vertically
                    Margin = new Thickness(0, 0, 10, 0) // Right margin for spacing
                }
                .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = ViewModel.AddToDoCommand
                }))
            }
        };
    }

    private DataTemplate ToDoItemTemplate()
    {
        return new DataTemplate(() =>
        {
            var titleLabel = new Label()
                .Font(size: 18)
                .TextColor(Colors.Black)
                .Bind(Label.TextProperty, nameof(ToDoItem.Title));

            var deleteSwipeItem = new SwipeItem
            {
                Text = "Delete",
                BackgroundColor = Color.FromRgba("#999999"),
                IconImageSource = "delete_icon.png"        
            };

            deleteSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding(nameof(ViewModel.DeleteTaskCommand), source: this.BindingContext));
            deleteSwipeItem.SetBinding(SwipeItem.CommandParameterProperty, new Binding("."));

            var swipeView = new SwipeView
            {
                Threshold = 100,
                Padding = 0,
                RightItems = new SwipeItems { deleteSwipeItem }.Invoke(s => s.Mode = SwipeMode.Execute),
                Content = new Frame
                {
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 5),
                    CornerRadius = 8,
                    BackgroundColor = Colors.White,
                    Content = titleLabel
                }
            };

            swipeView.SwipeStarted += (s, e) =>
            {
                ((Frame)swipeView.Content).BackgroundColor = Colors.LightGray; // Change color on swipe start
            };

            swipeView.SwipeEnded += (s, e) =>
            {
                ((Frame)swipeView.Content).BackgroundColor = Colors.White; // Reset color on swipe end
            };

            swipeView.Content.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var tappedItem = (ToDoItem)swipeView.BindingContext;
                    if (tappedItem != null)
                    {
                        await ViewModel.OpenDetails(tappedItem);
                    }
                }),
                CommandParameter = new Binding(".")
            });

            return swipeView;
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.LoadToDoItems(); // Refresh the to-do list on page appearance
    }
}
