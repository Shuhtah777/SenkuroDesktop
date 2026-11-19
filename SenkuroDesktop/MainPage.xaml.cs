using SenkuroDesktop.Models;
using SenkuroDesktop.Services;

namespace SenkuroDesktop;

public partial class MainPage : ContentPage
{
    private string _currentFilter = "Все";

    public MainPage()
    {
        InitializeComponent();
        LoadCatalog();
    }

    private void LoadCatalog()
    {
        var items = MockCatalogService.GetTitles(SearchEntry?.Text, _currentFilter);
        CatalogCollectionView.ItemsSource = items;
        CountLabel.Text = items.Count.ToString();
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        LoadCatalog();
    }

    private void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is not Button clickedBtn) return;

        _currentFilter = clickedBtn.Text;

        // Сбрасываем стили всех кнопок фильтра
        var allButtons = new[] { FilterAllBtn, FilterMangaBtn, FilterManhwaBtn, FilterRanobeBtn };
        var inactiveBg = (Color)Application.Current!.Resources["CardBackgroundColor"];
        var inactiveText = (Color)Application.Current!.Resources["TextSecondaryColor"];
        var activeBg = (Color)Application.Current!.Resources["AccentPrimaryColor"];

        foreach (var btn in allButtons)
        {
            btn.BackgroundColor = inactiveBg;
            btn.TextColor = inactiveText;
            btn.FontAttributes = FontAttributes.None;
        }

        // Выделяем активную кнопку
        clickedBtn.BackgroundColor = activeBg;
        clickedBtn.TextColor = Colors.White;
        clickedBtn.FontAttributes = FontAttributes.Bold;

        LoadCatalog();
    }

    // Эффект наведения мыши для Windows
    private void OnCardPointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Stroke = (Color)Application.Current!.Resources["AccentPrimaryColor"];
            border.ScaleTo(1.02, 120, Easing.CubicOut);
        }
    }

    private void OnCardPointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Stroke = (Color)Application.Current!.Resources["BorderSubtleColor"];
            border.ScaleTo(1.0, 120, Easing.CubicOut);
        }
    }

    // Клик по карточке тайтла
    private async void OnTitleTapped(object? sender, TappedEventArgs e)
    {
        TitleItem? selectedTitle = e.Parameter as TitleItem;

        if (selectedTitle is null && sender is BindableObject bindable)
        {
            selectedTitle = bindable.BindingContext as TitleItem;
        }

        if (selectedTitle is null)
            return;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Navigation is not null)
            {
                await Navigation.PushAsync(new Views.TitleDetailPage(selectedTitle));
            }
            else if (Shell.Current is not null)
            {
                await Shell.Current.Navigation.PushAsync(new Views.TitleDetailPage(selectedTitle));
            }
        });
    }
}