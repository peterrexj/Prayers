namespace CatholicPrayers.Maui.Services;

public static class ServiceLocator
{
    private static IServiceProvider? _provider;

    public static void Init(IServiceProvider provider) => _provider = provider;

    public static T? Get<T>() where T : class
    {
        try { return _provider?.GetService(typeof(T)) as T; }
        catch { return null; }
    }

    public static T GetRequired<T>() where T : class
        => _provider!.GetRequiredService<T>() as T
           ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found.");
}
