namespace InstaDelivery.Common.Context;


public class RequestContext
{
    private static readonly AsyncLocal<RequestContext> _current = new AsyncLocal<RequestContext>();

    public static RequestContext Current
    {
        get => _current.Value;
        set => _current.Value = value;
    }

    public string? UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? UserName { get; set; }

    public static void Reset() => _current.Value = null;
}
