namespace MyTransformationWeb.Services.Internal;

public class SimplePagination<T> : IPaginationService<T> where T : class
{
    private int CurrentPage = 1;

    private readonly Dictionary<int, IEnumerable<T>> Pages = [];

    public Dictionary<int, IEnumerable<T>> GetPages(int maxElementsByPage, IEnumerable<T> list)
    {
        Pages.Clear();

        if (list.Count() <= maxElementsByPage)
        {
            Pages.Add(1, list);
            return Pages;
        }

        var totalPages = Math.Ceiling(list.Count() / (double)maxElementsByPage);

        for (int i = 0; i < totalPages; i++)
        {
            Pages.Add(i + 1, list.Skip(i * maxElementsByPage).Take(maxElementsByPage));
        }

        return Pages;
    }

    public int NextPage()
    {
        if (CurrentPage == Pages.Count)
        {
            return CurrentPage;
        }

        CurrentPage++;
        return CurrentPage;
    }

    public int PreviousPage()
    {
        if (CurrentPage == 1)
        {
            return CurrentPage;
        }

        CurrentPage--;
        return CurrentPage;
    }
}
