using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Prayers.Droid.Services;
using Prayers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            }

        }

        private void _mediaPlayer_Prepared(object sender, EventArgs e)
        {
            _mediaPlayer.Start();
        }

        //private void

        public void Pause()
        {
            if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }
        }

        public void Stop()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                //_mediaPlayer.Release();
                ReleasePlayer();
            }
        }
    }
}