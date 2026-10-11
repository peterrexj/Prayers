using AVFoundation;
using Foundation;
using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.Platforms.iOS;

public class AudioService : IAudioService
{
    private AVAudioPlayer? _player;
    private Queue<string> _queue = new();

    public bool IsPlaying => _player?.Playing == true;

    public async Task Play(List<string> audioFiles)
    {
        if (_player?.Playing == true) return;
        if (_player != null && !_player.Playing) { _player.Play(); return; }

        _queue.Clear();
        foreach (var f in audioFiles) _queue.Enqueue(f);
        PlayNext();
        await Task.CompletedTask;
    }

    private void PlayNext()
    {
        if (_queue.Count == 0) { AudioComplete(); return; }
        var file = _queue.Dequeue();
        if (string.IsNullOrEmpty(file)) { PlayNext(); return; }

        var fileName = Path.GetFileNameWithoutExtension(file);
        var ext = Path.GetExtension(file).TrimStart('.');
        // Files are in the Audio/ subfolder in the bundle
        var path = NSBundle.MainBundle.PathForResource(fileName, ext, "Audio")
                ?? NSBundle.MainBundle.PathForResource(fileName, ext);
        if (path == null) { PlayNext(); return; }

        var url = NSUrl.FromString(path);
        if (url == null) { PlayNext(); return; }

        _player?.Dispose();
        _player = AVAudioPlayer.FromUrl(url);
        if (_player == null) { PlayNext(); return; }

        _player.FinishedPlaying += (s, e) =>
        {
            ReleasePlayer();
            PlayNext();
        };
        _player.PrepareToPlay();
        _player.Play();
    }

    public void Pause()
    {
        if (_player?.Playing == true) _player.Pause();
    }

    public void Stop()
    {
        ReleasePlayer();
        _queue.Clear();
    }

    public void AudioComplete() { }

    private void ReleasePlayer()
    {
        _player?.Stop();
        _player?.Dispose();
        _player = null;
    }
}
