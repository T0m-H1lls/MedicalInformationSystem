using Medical_information_system.ViewModels;

namespace Medical_information_system;

public class Navigation
{
    private StartViewModel startViewModel;
    
    public void Navigate(ViewModelBase viewModel)
    {
        startViewModel.CurrentPage = viewModel;
    }

    public void SetCurrentView(StartViewModel startViewModel)
    {
        this.startViewModel = startViewModel;
    }

    public void Close()
    {
        this.startViewModel.Close();
    }
}