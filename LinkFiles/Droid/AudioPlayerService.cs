using Android.Media;
using Microsoft.AppCenter.Crashes;
using Pj.Library;
using Prayers.Droid.Services;
using Prayers.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Prayers.Droid.Services
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
            _mediaPlayer.Prepared += _mediaPlayer_Prepared;
            _mediaPlayer.Completion += _mediaPlayer_Completion;
        }

        private void ReleasePlayer()
        {
            _mediaPlayer.Release();
            _mediaPlayer.Prepared -= _mediaPlayer_Prepared;
            _mediaPlayer.Completion -= _mediaPlayer_Completion;
            _mediaPlayer = null;
        }
        private async void _mediaPlayer_Completion(object sender, EventArgs e)
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

        public async Task Play(List<string> audioFilePath)
        {
            try
            {
                if (_mediaPlayer != null && _mediaPlayer.IsPlaying) return;
                if (_mediaPlayer != null && _mediaPlayer.IsPlaying == false)
                {
                    _mediaPlayer.Start();
                }
                else
                {
                    _mediaFilesToPlay.Clear();
                    audioFilePath.Iter(f => _mediaFilesToPlay.Enqueue(f));
                    await CheckAudioToPlay();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
            var fd = global::Android.App.Application.Context.Assets.OpenFd(file);
            await _mediaPlayer.SetDataSourceAsync(fd.FileDescriptor, fd.StartOffset, fd.Length);
            _mediaPlayer.Prepare();
        }

        private void _mediaPlayer_Prepared(object sender, EventArgs e)
        {
            try
            {
                _mediaPlayer.Start();
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
                if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
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
                    _mediaPlayer.Stop();
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