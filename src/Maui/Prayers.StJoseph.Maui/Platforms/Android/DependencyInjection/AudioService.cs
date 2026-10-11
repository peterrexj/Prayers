using Android.Media;
using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.Platforms.Android.DependencyInjection;

public class AudioService : IAudioService
{
    private MediaPlayer? _player;
    private Queue<string> _queue = new();

    public bool IsPlaying => _player?.IsPlaying == true;

    public async Task Play(List<string> audioFiles)
    {
        if (_player != null && _player.IsPlaying) return;
        if (_player != null && !_player.IsPlaying) { _player.Start(); return; }

        _queue.Clear();
        foreach (var f in audioFiles) _queue.Enqueue(f);
        await PlayNext();
    }

    private async Task PlayNext()
    {
        if (_queue.Count == 0) { AudioComplete(); return; }
        var file = _queue.Dequeue();
        if (string.IsNullOrEmpty(file)) { await PlayNext(); return; }

        _player = new MediaPlayer();
        _player.Prepared += (s, e) => _player.Start();
        _player.Completion += async (s, e) => { ReleasePlayer(); await PlayNext(); };

        // MAUI bundles Raw assets preserving subdirectory: Audio/filename.mp3
        var assetPath = file.Contains('/') ? file : $"Audio/{file}";
        var fd = global::Android.App.Application.Context.Assets?.OpenFd(assetPath);
        if (fd == null) return;
        await _player.SetDataSourceAsync(fd.FileDescriptor, fd.StartOffset, fd.Length);
        _player.Prepare();
    }

    public void Pause()
    {
        if (_player?.IsPlaying == true) _player.Pause();
    }

    public void Stop()
    {
        if (_player != null) ReleasePlayer();
        _queue.Clear();
    }

    public void AudioComplete() { }

    private void ReleasePlayer()
    {
        _player?.Release();
        _player = null;
    }
}
