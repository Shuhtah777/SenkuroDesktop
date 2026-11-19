using SenkuroDesktop.Models;

namespace SenkuroDesktop.Services;

public static class MockCatalogService
{
    private static readonly List<TitleItem> _titles = new()
    {
        new TitleItem
        {
            Title = "Поднятие уровня в одиночку",
            OriginalTitle = "Solo Leveling",
            Type = "Манхва",
            Status = "Завершён",
            Year = 2018,
            Rating = 9.6,
            CoverPlaceholderColor = Color.FromArgb("#282147"),
            Genres = new() { "Экшен", "Фэнтези", "Приключения", "Подземелья" },
            Description = "Десять лет назад открылись Врата, соединившие реальный мир с миром монстров. Обычные люди получили сверхспособности и стали Охотниками. Сон Джин-Ву — самый слабый охотник E-ранга, который едва сводит концы с концами, но однажды в двойном подземелье он получает таинственную Систему.",
            Chapters = GenerateChapters(179)
        },
        new TitleItem
        {
            Title = "Берсерк",
            OriginalTitle = "Berserk",
            Type = "Манга",
            Status = "Онгоинг",
            Year = 1989,
            Rating = 9.8,
            CoverPlaceholderColor = Color.FromArgb("#3D1E24"),
            Genres = new() { "Тёмное фэнтези", "Драма", "Экшен", "Сэйнэн" },
            Description = "Гатс, известный как Чёрный Мечник, странствует по мрачному средневековому миру, охотясь на демонических Апостолов в поисках мести человеку, которого когда-то считал своим лучшим другом и лидером.",
            Chapters = GenerateChapters(375)
        },
        new TitleItem
        {
            Title = "Всеведущий читатель",
            OriginalTitle = "Omniscient Reader's Viewpoint",
            Type = "Манхва",
            Status = "Онгоинг",
            Year = 2020,
            Rating = 9.5,
            CoverPlaceholderColor = Color.FromArgb("#1E3246"),
            Genres = new() { "Фэнтези", "Апокалипсис", "Экшен", "Психология" },
            Description = "Ким Докча — обычный офисный работник, единственным увлечением которого было чтение никому не известного веб-романа. В день, когда выходит последняя глава, сюжет книги внезапно становится реальностью.",
            Chapters = GenerateChapters(210)
        },
        new TitleItem
        {
            Title = "Реинкарнация безработного",
            OriginalTitle = "Mushoku Tensei",
            Type = "Ранобэ",
            Status = "Завершён",
            Year = 2014,
            Rating = 9.1,
            CoverPlaceholderColor = Color.FromArgb("#233A2E"),
            Genres = new() { "Исекай", "Магия", "Приключения", "Драма" },
            Description = "34-летний затворник погибает под колёсами грузовика и перерождается в мире меча и магии в теле младенца Рудеуса Грейрата, сохранив все воспоминания прошлой жизни и решив прожить новую без сожалений.",
            Chapters = GenerateChapters(260)
        },
        new TitleItem
        {
            Title = "Человек-бензопила",
            OriginalTitle = "Chainsaw Man",
            Type = "Манга",
            Status = "Онгоинг",
            Year = 2018,
            Rating = 9.2,
            CoverPlaceholderColor = Color.FromArgb("#4A251B"),
            Genres = new() { "Экшен", "Сверхъестественное", "Сёнэн", "Ужасы" },
            Description = "Дэндзи вынужден выплачивать огромные долги покойного отца, работая охотником на демонов вместе со своим верным демоном-псом Почитой. После предательства он сливается с Почитой и обретает силу бензопил.",
            Chapters = GenerateChapters(170)
        },
        new TitleItem
        {
            Title = "Начало после конца",
            OriginalTitle = "The Beginning After The End",
            Type = "Манхва",
            Status = "Онгоинг",
            Year = 2018,
            Rating = 9.4,
            CoverPlaceholderColor = Color.FromArgb("#2E284E"),
            Genres = new() { "Фэнтези", "Реинкарнация", "Магия", "Экшен" },
            Description = "Король Грей обладал непревзойдённой силой, богатством и авторитетом в мире боевых искусств, но одиночество всегда следовало за ним по пятам. После загадочной смерти он перерождается на континенте магии.",
            Chapters = GenerateChapters(185)
        },
        new TitleItem
        {
            Title = "Провожающая в последний путь Фрирен",
            OriginalTitle = "Sousou no Frieren",
            Type = "Манга",
            Status = "Онгоинг",
            Year = 2020,
            Rating = 9.6,
            CoverPlaceholderColor = Color.FromArgb("#253942"),
            Genres = new() { "Приключения", "Фэнтези", "Драма", "Повседневность" },
            Description = "Отряд героев одолел Короля Демонов и вернулся домой. Для эльфийки-мага Фрирен десять лет странствий были лишь коротким мгновением, но время беспощадно к её человеческим спутникам.",
            Chapters = GenerateChapters(130)
        },
        new TitleItem
        {
            Title = "Повелитель Тайн",
            OriginalTitle = "Lord of the Mysteries",
            Type = "Ранобэ",
            Status = "Завершён",
            Year = 2018,
            Rating = 9.7,
            CoverPlaceholderColor = Color.FromArgb("#33233D"),
            Genres = new() { "Стимпанк", "Мистика", "Детектив", "Фэнтези" },
            Description = "В эпоху паровых машин и дредноутов Чжоу Минжуй приходит в себя в теле Клейна Моретти — выпускника исторического факультета в викторианском мире, полном скрытых культов и потусторонних сил.",
            Chapters = GenerateChapters(1432)
        }
    };

    public static List<TitleItem> GetTitles(string? searchQuery = null, string? typeFilter = null)
    {
        IEnumerable<TitleItem> query = _titles;

        if (!string.IsNullOrWhiteSpace(typeFilter) && typeFilter != "Все")
        {
            query = query.Where(t => t.Type.Equals(typeFilter, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(t =>
                t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.OriginalTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
        }

        return query.ToList();
    }

    private static List<ChapterItem> GenerateChapters(int totalCount)
    {
        var list = new List<ChapterItem>();
        // Сгенерируем последние 15 глав для демонстрации списка (от свежих к старым)
        int start = Math.Max(1, totalCount - 14);
        for (int i = totalCount; i >= start; i--)
        {
            list.Add(new ChapterItem
            {
                Number = i,
                Volume = $"Том {(i / 10) + 1}",
                Title = i == totalCount ? "Переломный момент" : $"Глава {i}",
                ReleaseDate = DateTime.Now.AddDays(-(totalCount - i) * 3).ToString("dd.MM.yyyy")
            });
        }
        return list;
    }
}