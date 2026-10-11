namespace Prayers.StJoseph.Maui.Services;

public interface IAudioService
{
    Task Play(List<string> audioFiles);
    void Pause();
    void Stop();
    bool IsPlaying { get; }
    void AudioComplete();
}
