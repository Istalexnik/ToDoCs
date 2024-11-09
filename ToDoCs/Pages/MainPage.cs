using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCs.Pages;
using ToDoCs.ViewModels;

namespace ToDoCs;
  class MainPage : BaseContentPage<MainViewModel>

{
    public MainPage(MainViewModel mainViewModel) : base (mainViewModel, "My Main Page")
    {
        
    }
}
