using Microsoft.Maui.Controls;
using SenkuroDesktop.Models;

namespace SenkuroDesktop.Views;

public partial class ChapterReaderPage : ContentPage
{
    private readonly TitleItem _title;
    private readonly List<ChapterItem> _orderedChapters;
    private int _currentIndex;

    public ChapterReaderPage(TitleItem title, ChapterItem startChapter)
    {
        InitializeComponent();
        _title = title;

        // В MockCatalogService главы идут по убыванию номера; развернём для логики "пред. / след."
        _orderedChapters = title.Chapters.Count > 0
            ? title.Chapters.OrderBy(c => c.Number).ToList()
            : new List<ChapterItem> { startChapter };

        _currentIndex = _orderedChapters.FindIndex(c => c.Number == startChapter.Number);
        if (_currentIndex < 0)
        {
            _orderedChapters.Add(startChapter);
            _orderedChapters = _orderedChapters.OrderBy(c => c.Number).ToList();
            _currentIndex = _orderedChapters.FindIndex(c => c.Number == startChapter.Number);
        }

        LoadChapter();
    }

    private ChapterItem CurrentChapter => _orderedChapters[_currentIndex];

    private void LoadChapter()
    {
        var chapter = CurrentChapter;

        ReaderTitleLabel.Text = _title.Title;
        ReaderChapterLabel.Text = $"{chapter.Volume} · Глава {chapter.Number}" +
            (string.IsNullOrWhiteSpace(chapter.Title) ? string.Empty : $" — {chapter.Title}");
        BottomChapterInfoLabel.Text = $"Глава {chapter.Number} ({chapter.ReleaseDate})";

        PrevChapterButton.IsEnabled = _currentIndex > 0;
        NextChapterButton.IsEnabled = _currentIndex < _orderedChapters.Count - 1;

        const int pageCount = 6;
        PagesCountLabel.Text = $"Фрагментов: {pageCount}";

        PagesContainer.Children.Clear();
        for (int page = 1; page <= pageCount; page++)
        {
            PagesContainer.Children.Add(BuildPageCard(chapter, page, pageCount));
        }

        ReaderScrollView.ScrollToAsync(0, 0, false);
    }

    private Border BuildPageCard(ChapterItem chapter, int pageNumber, int totalPages)
    {
        var card = new Border
        {
            BackgroundColor = Color.FromArgb("#1D1D26"),
            Stroke = Color.FromArgb("#2A2A37"),
            StrokeThickness = 1,
            HeightRequest = 420,
            Padding = new Thickness(28),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 }
        };

        var grid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            }
        };

        var headerLabel = new Label
        {
            Text = $"{_title.Type} · {_title.Title}",
            TextColor = Color.FromArgb("#7C5CFF"),
            FontSize = 12,
            FontAttributes = FontAttributes.Bold
        };

        var centerStack = new VerticalStackLayout
        {
            Spacing = 10,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        centerStack.Children.Add(new Label
        {
            Text = $"Глава {chapter.Number}",
            TextColor = Color.FromArgb("#F4F4F8"),
            FontSize = 32,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        });

        centerStack.Children.Add(new Label
        {
            Text = string.IsNullOrWhiteSpace(chapter.Title)
                ? $"Фрагмент {pageNumber} из {totalPages}"
                : $"{chapter.Title} — фрагмент {pageNumber}",
            TextColor = Color.FromArgb("#A1A1B5"),
            FontSize = 15,
            HorizontalOptions = LayoutOptions.Center
        });

        centerStack.Children.Add(new Label
        {
            Text = "Заглушка страницы главы для локального режима без подключения к источнику",
            TextColor = Color.FromArgb("#6E6E82"),
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center
        });

        var footerLabel = new Label
        {
            Text = $"Стр. {pageNumber} / {totalPages}",
            TextColor = Color.FromArgb("#6E6E82"),
            FontSize = 12,
            HorizontalOptions = LayoutOptions.End
        };

        grid.Add(headerLabel, 0, 0);
        grid.Add(centerStack, 0, 1);
        grid.Add(footerLabel, 0, 2);
        card.Content = grid;

        return card;
    }

    private async void OnPrevChapterClicked(object? sender, EventArgs e)
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            LoadChapter();
            await Task.CompletedTask;
        }
    }

    private async void OnNextChapterClicked(object? sender, EventArgs e)
    {
        if (_currentIndex < _orderedChapters.Count - 1)
        {
            _currentIndex++;
            LoadChapter();
            await Task.CompletedTask;
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}