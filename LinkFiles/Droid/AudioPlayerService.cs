using Android.Media;
using Microsoft.AppCenter.Crashes;
using Prayers.Droid.Services;
using Prayers.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Prayers.Droid.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer _mediaPlayer;

        public AudioPlayerService()
        {
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
        private void _mediaPlayer_Completion(object sender, EventArgs e)
        {
            SharedServices.AudioController.AudioComplete();
            ReleasePlayer();
        }

        public async Task Play(string audioFilePath)
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
                    InitializePlayer();
                    var fd = global::Android.App.Application.Context.Assets.OpenFd(audioFilePath);
                    await _mediaPlayer.SetDataSourceAsync(fd.FileDescriptor, fd.StartOffset, fd.Length);
                    _mediaPlayer.Prepare();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        private void _mediaPlayer_Prepared(object sender, EventArgs e)
        {
            _mediaPlayer.Start();
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