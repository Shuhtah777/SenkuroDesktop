using SenkuroDesktop.Models;

namespace SenkuroDesktop.Views;

public partial class TitleDetailPage : ContentPage
{
    private readonly TitleItem _title;
    private bool _isBookmarked;

    public TitleDetailPage(TitleItem title)
    {
        InitializeComponent();
        _title = title;
        BindTitleData();
    }

    private void BindTitleData()
    {
        TopBarTitleLabel.Text = _title.Title;
        TitleLabel.Text = _title.Title;
        OriginalTitleLabel.Text = string.IsNullOrWhiteSpace(_title.OriginalTitle)
            ? string.Empty
            : _title.OriginalTitle;

        TypeLabel.Text = _title.Type;
        StatusLabel.Text = _title.Status;
        YearLabel.Text = _title.Year.ToString();
        RatingLabel.Text = $"? {_title.Rating:F1}";
        DescriptionLabel.Text = _title.Description;

        CoverBorder.BackgroundColor = _title.CoverPlaceholderColor;
        CoverLetterLabel.Text = string.IsNullOrEmpty(_title.Title)
            ? "?"
            : _title.Title[0].ToString().ToUpper();

        // Жанры
        GenresFlexLayout.Children.Clear();
        var borderBg = (Color)Application.Current!.Resources["BadgeBackgroundColor"];
        var textSecondary = (Color)Application.Current!.Resources["TextSecondaryColor"];
        var borderSubtle = (Color)Application.Current!.Resources["BorderSubtleColor"];

        foreach (var genre in _title.Genres)
        {
            var chip = new Border
            {
                BackgroundColor = borderBg,
                Stroke = borderSubtle,
                StrokeThickness = 1,
                Padding = new Thickness(12, 5),
                Margin = new Thickness(0, 0, 8, 8),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = new Label
                {
                    Text = genre,
                    TextColor = textSecondary,
                    FontSize = 12
                }
            };
            GenresFlexLayout.Children.Add(chip);
        }

        // Список глав
        ChaptersCountLabel.Text = _title.Chapters.Count.ToString();
        ChaptersContainer.Children.Clear();

        for (int i = 0; i < _title.Chapters.Count; i++)
        {
            var chapter = _title.Chapters[i];
            var rowGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition(new GridLength(95)),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                Padding = new Thickness(16, 12),
                ColumnSpacing = 12
            };

            var volLabel = new Label
            {
                Text = chapter.Volume,
                TextColor = textSecondary,
                FontSize = 13,
                VerticalOptions = LayoutOptions.Center
            };

            var titleLabel = new Label
            {
                Text = string.IsNullOrWhiteSpace(chapter.Title)
                    ? $"Глава {chapter.Number}"
                    : $"Глава {chapter.Number} — {chapter.Title}",
                TextColor = (Color)Application.Current!.Resources["TextPrimaryColor"],
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            var dateLabel = new Label
            {
                Text = chapter.ReleaseDate,
                TextColor = (Color)Application.Current!.Resources["TextMutedColor"],
                FontSize = 12,
                VerticalOptions = LayoutOptions.Center
            };

            rowGrid.Add(volLabel, 0, 0);
            rowGrid.Add(titleLabel, 1, 0);
            rowGrid.Add(dateLabel, 2, 0);

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) => await OpenChapterAsync(chapter);
            rowGrid.GestureRecognizers.Add(tap);

            var pointer = new PointerGestureRecognizer();
            var defaultBg = Colors.Transparent;
            var hoverBg = Color.FromArgb("#252532");
            pointer.PointerEntered += (_, _) => rowGrid.BackgroundColor = hoverBg;
            pointer.PointerExited += (_, _) => rowGrid.BackgroundColor = defaultBg;
            rowGrid.GestureRecognizers.Add(pointer);

            ChaptersContainer.Children.Add(rowGrid);

            if (i < _title.Chapters.Count - 1)
            {
                ChaptersContainer.Children.Add(new BoxView
                {
                    HeightRequest = 1,
                    Color = borderSubtle
                });
            }
        }
    }

    private async Task OpenChapterAsync(ChapterItem chapter)
    {
        await Navigation.PushAsync(new ChapterReaderPage(_title, chapter));
    }

    private async void OnReadClicked(object? sender, EventArgs e)
    {
        var firstChapter = _title.Chapters.OrderBy(c => c.Number).FirstOrDefault()
            ?? new ChapterItem { Number = 1, Volume = "Том 1", Title = "Начало", ReleaseDate = DateTime.Now.ToString("dd.MM.yyyy") };
        await OpenChapterAsync(firstChapter);
    }

    private void OnBookmarkClicked(object? sender, EventArgs e)
    {
        _isBookmarked = !_isBookmarked;
        BookmarkButton.Text = _isBookmarked ? "В закладках ?" : "В закладки";
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}