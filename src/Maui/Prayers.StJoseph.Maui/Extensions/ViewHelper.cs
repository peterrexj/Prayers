namespace Prayers.StJoseph.Maui.Extensions;

public static class ViewHelper
{
    public static void RunOnMainThread(Action action)
    {
        if (MainThread.IsMainThread)
            action();
        else
            MainThread.BeginInvokeOnMainThread(action);
    }
}
