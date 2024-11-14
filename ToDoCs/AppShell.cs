using ToDoCs.Pages;

namespace ToDoCs;
class AppShell : Shell
{
    private static readonly Dictionary<Type, string> RouteMap = new()
    {
        {typeof(MainPage), "main" },
        {typeof(DetailsPage), "details" },
        {typeof(AddTaskPage), "addtask" },
    };

    public AppShell(MainPage mainPage)
    {
        Items.Add(mainPage);

        foreach(var route in RouteMap)
        {
            Routing.RegisterRoute(route.Value, route.Key);
        }

        Shell.SetBackgroundColor(this, Color.FromRgba("#999999"));
        Shell.SetTitleColor(this, Colors.White);

    }

    public static string? GetRoute(Type type)
    {
        return RouteMap.TryGetValue(type, out var route) ? route : null;
    }
}
