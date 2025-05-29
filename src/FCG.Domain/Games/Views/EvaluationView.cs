namespace FCG.Domain.Games.Contracts;
public class EvaluationView
{
    public Guid Key { get; set; }
    public EFiveStars Stars { get; set; }
    public string? Comment { get; set; }
    public Guid UserKey { get; set; }
    public string UserFullName { get; set; }
}
