namespace Prayers.StJoseph.Maui.Services;

public static class ServiceLocator
{
    public static IServiceProvider? ServiceProvider { get; set; }

    public static T? GetService<T>() where T : class
        => ServiceProvider?.GetService<T>();

    public static T GetRequiredService<T>() where T : class
        => ServiceProvider!.GetRequiredService<T>();
}
