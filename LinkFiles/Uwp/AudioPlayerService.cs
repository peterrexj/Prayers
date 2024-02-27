using Microsoft.AppCenter.Crashes;
using Pj.Library;
using Prayers.Services;
using Prayers.UWP.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Prayers.UWP.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer _mediaPlayer;
        private Queue<string> _mediaFilesToPlay;

        public AudioPlayerService()
        {
            _mediaFilesToPlay = new Queue<string>();
        }

        private void InitializePlayer()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
        }
        private void ReleasePlayer()
        {
            _mediaPlayer.MediaEnded -= _mediaPlayer_MediaEnded;
            _mediaPlayer.Source = null;
            _mediaPlayer = null;
        }

        private async Task CheckAudioToPlay()
        {
            if (_mediaFilesToPlay.Count <= 0)
            {
                SharedServices.AudioController.AudioComplete();
                return;
            }
            var file = _mediaFilesToPlay.Dequeue();
            if (file.IsEmpty()) return;

            InitializePlayer();
            Uri uri = new Uri($"ms-appx:///Assets/{file}", UriKind.Absolute);
            StorageFile localStorageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            if (localStorageFile != null)
            {
                var stream = await localStorageFile.OpenAsync(FileAccessMode.Read);
                _mediaPlayer.Source = MediaSource.CreateFromStream(stream, localStorageFile.ContentType);
                _mediaPlayer.Play();
            }
        }
        private async void _mediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            try
            {
                ReleasePlayer();
                await CheckAudioToPlay();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public bool IsMediaPlaying() => _mediaPlayer?.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;

        public async Task Play(List<string> pathToAudio)
        {
            try
            {
                if (_mediaPlayer != null && IsMediaPlaying()) return;
                if (_mediaPlayer != null && IsMediaPlaying() == false)
                {
                    _mediaPlayer.Play();
                }
                else
                {
                    _mediaFilesToPlay.Clear();
                    pathToAudio.Iter(f => _mediaFilesToPlay.Enqueue(f));
                    await CheckAudioToPlay();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        public void Pause()
        {
            try
            {
                if (_mediaPlayer != null && IsMediaPlaying())
                {
                    _mediaPlayer.Pause();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Stop()
        {
            try
            {
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Pause();
                    ReleasePlayer();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
