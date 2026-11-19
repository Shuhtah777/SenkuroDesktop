namespace SenkuroDesktop.Models;

public class TitleItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Type { get; set; } = "Манга"; // Манга, Манхва, Ранобэ
    public string Status { get; set; } = "Онгоинг";
    public int Year { get; set; }
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
    public List<ChapterItem> Chapters { get; set; } = new();

    // Цвет-заглушка для обложки, пока нет реальных картинок с сети
    public Color CoverPlaceholderColor { get; set; } = Color.FromArgb("#2E2B4F");

    // Если захочешь подставить реальную ссылку или локальный файл
    public string? CoverUrl { get; set; }
}

public class ChapterItem
{
    public int Number { get; set; }
    public string Volume { get; set; } = "Том 1";
    public string Title { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
}