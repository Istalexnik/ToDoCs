using ToDoCs.ViewModels;

namespace ToDoCs.Pages;
abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    protected TViewModel ViewModel => (TViewModel)BindingContext;

    protected BaseContentPage(TViewModel viewModel, string pageTitle)
    {
        //Title = pageTitle;
        BindingContext = viewModel;
    }
}
