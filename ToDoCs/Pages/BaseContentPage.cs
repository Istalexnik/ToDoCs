using ToDoCs.ViewModels;

namespace ToDoCs.Pages;
abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    protected new TViewModel BindingContext => (TViewModel)base.BindingContext;
    protected BaseContentPage(TViewModel viewModel, string pageTitle)
    {
        //Title = pageTitle;
        base.BindingContext = viewModel;
    }
}
