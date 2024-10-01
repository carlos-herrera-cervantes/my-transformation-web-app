namespace MyTransformationWeb.Services.Internal;

public interface IPaginationService<T> where T : class
{
    Dictionary<int, IEnumerable<T>> GetPages(int maxElementsByPage, IEnumerable<T> list);

    int NextPage();

    int PreviousPage();
}
