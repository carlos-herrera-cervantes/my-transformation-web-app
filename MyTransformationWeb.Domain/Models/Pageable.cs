namespace MyTransformationWeb.Domain.Models;

public class Pageable
{
    public int Page { get; set; } = 0;

    public int PageSize { get; set; } = 10;

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public bool NoExercise { get; set; } = false;
}
