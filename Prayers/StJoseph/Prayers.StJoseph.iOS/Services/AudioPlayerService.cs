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

        public async Task Play(string audioFilePath)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(audioFilePath), Path.GetExtension(audioFilePath));
            var url = NSUrl.FromString(sFilePath);
            var _mediaPlayer = AVAudioPlayer.FromUrl(url);
            _mediaPlayer.FinishedPlaying += (object sender, AVStatusEventArgs e) => {
                _mediaPlayer = null;
            };
            _mediaPlayer.Play();
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
                _mediaPlayer.Stop();
                _mediaPlayer = null;
            }
        }
    }
}