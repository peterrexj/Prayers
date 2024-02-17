using AVFoundation;
using Foundation;
using Prayers.iOS.Services;
using Prayers.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Prayers.iOS.Services
{
    //https://appsmithsllc.com/playing-embedded-audio-files-in-xamarin-forms/
    //check the ios part
    public class AudioPlayerService : IAudioPlayerService
    {
        private AVAudioPlayer _mediaPlayer;

        private void InitializePlayer(string fileName)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
            var url = NSUrl.FromString(sFilePath);
            _mediaPlayer = AVAudioPlayer.FromUrl(url);
            _mediaPlayer.FinishedPlaying += _mediaPlayer_FinishedPlaying;
        }

        private void _mediaPlayer_FinishedPlaying(object sender, AVStatusEventArgs e)
        {
            SharedServices.AudioController.AudioComplete();
            ReleasePlayer();
        }

        private void ReleasePlayer()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.FinishedPlaying -= _mediaPlayer_FinishedPlaying;
            _mediaPlayer = null;
        }

        public async Task Play(string audioFilePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_mediaPlayer != null && _mediaPlayer.Playing) return;
                    if (_mediaPlayer != null && _mediaPlayer.Playing == false)
                    {
                        _mediaPlayer.Play();
                    }
                    else
                    {
                        InitializePlayer(audioFilePath);
                        _mediaPlayer.PrepareToPlay();
                        _mediaPlayer.Play();
                    }
                });
                
            }
            catch (Exception ex)
            {

                
            }
        }

        public void Pause()
        {
            if (_mediaPlayer != null && _mediaPlayer.Playing)
            {
                _mediaPlayer.Pause();
            }
        }

        public void Stop()
        {
            if (_mediaPlayer != null)
            {
                ReleasePlayer();
            }
        }
    }
}