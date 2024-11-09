using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCs;
class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        Items.Add(mainPage);

        foreach(var route in RouteMap)
        {
            Routing.RegisterRoute(route.Value, route.Key);
        }
    }

    private static readonly Dictionary<Type, string> RouteMap = new()
    {
        {typeof(MainPage), "main" }
    };

    public static string? GetRoute(Type type)
    {
        return RouteMap.TryGetValue(type, out var route) ? route : null;
    }
}
