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

                    // Replace HorizontalStackLayout with Grid
                    

                    new CollectionView
                    {
                        ItemsLayout = LinearItemsLayout.Vertical,
                        ItemTemplate = ToDoItemTemplate()
                    }
                    .Bind(CollectionView.ItemsSourceProperty, nameof(ViewModel.ToDoItems))
                }
            }
        },

        // Floating action button positioned in the bottom-right corner
        new Frame
        {
            WidthRequest = 60,
            HeightRequest = 60,
            CornerRadius = 30, // Make it circular
            BackgroundColor = Colors.Blue, // Adjust to your preferred color
            Padding = 0,
            Content = new Label
            {
                Text = "+",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.White
            },
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 0, 20, 20) // Adjust margin as needed
        }
        .Invoke(f => f.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = ViewModel.AddToDoCommand
        }))
    }
        };



        var addTaskButton = new Frame
        {
            WidthRequest = 60,
            HeightRequest = 60,
            CornerRadius = 30, // Make it circular
            BackgroundColor = Colors.Blue, // Adjust to your preferred color
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            Padding = 0,
            Content = new Label
            {
                Text = "+",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Red
            }
        };

        addTaskButton.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = ViewModel.AddToDoCommand
        });



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
                BackgroundColor = Color.FromRgba("#777777"),
                IconImageSource = "delete_icon.png"
            };

            deleteSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding(nameof(ViewModel.DeleteTaskCommand), source: this.BindingContext));
            deleteSwipeItem.SetBinding(SwipeItem.CommandParameterProperty, new Binding("."));

            var swipeView = new SwipeView
            {
                Threshold = 100,
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
