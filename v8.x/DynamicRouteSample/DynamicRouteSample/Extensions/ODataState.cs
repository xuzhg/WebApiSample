namespace DynamicRouteSample.Extensions;

internal class ODataState
{
    public ODataState(string prefix, IServiceProvider serviceProvider)
    {
        Prefix = prefix;
        ServiceProvider = serviceProvider;
    }

    public string Prefix { get; }

    public IServiceProvider ServiceProvider { get; }
}
