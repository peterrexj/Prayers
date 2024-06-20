using AVFoundation;
using Foundation;
using Microsoft.AppCenter.Crashes;
using Pj.Library;
using Prayers.iOS.Services;
using Prayers.Services;
using System;
using System.Collections.Generic;
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
        private Queue<string> _mediaFilesToPlay;

        public AudioPlayerService()
        {
            _mediaFilesToPlay = new Queue<string>();
        }

        private void InitializePlayer(string fileName)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
            var url = NSUrl.FromString(sFilePath);
            _mediaPlayer = AVAudioPlayer.FromUrl(url);
            _mediaPlayer.FinishedPlaying += _mediaPlayer_FinishedPlaying;
        }

        private void _mediaPlayer_FinishedPlaying(object sender, AVStatusEventArgs e)
        {
            try
            {
                ReleasePlayer();
                CheckAudioToPlay();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ReleasePlayer()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.FinishedPlaying -= _mediaPlayer_FinishedPlaying;
            _mediaPlayer = null;
        }

        public async Task Play(List<string> audioFilePath)
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
                        _mediaFilesToPlay.Clear();
                        audioFilePath.Iter(f => _mediaFilesToPlay.Enqueue(f));
                        CheckAudioToPlay();
                    }
                });

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void CheckAudioToPlay()
        {
            if (_mediaFilesToPlay.Count <= 0)
            {
                SharedServices.AudioController.AudioComplete();
                return;
            }
            var file = _mediaFilesToPlay.Dequeue();
            if (file.IsEmpty()) return;

            InitializePlayer(file);
            _mediaPlayer.PrepareToPlay();
            _mediaPlayer.Play();
        }
         
        public void Pause()
        {
            try
            {
                if (_mediaPlayer == null || !_mediaPlayer.Playing) return;
                _mediaPlayer.Pause();
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
                if (_mediaPlayer == null) return;
                ReleasePlayer();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}