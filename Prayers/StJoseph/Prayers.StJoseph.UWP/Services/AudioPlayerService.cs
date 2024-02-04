using Prayers.Services;
using Prayers.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace Prayers.UWP.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private MediaPlayer mediaPlayer;

        public async Task Play(string audioFilePath)
        {
            StorageFolder Folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Sounds");
            StorageFile sf = await Folder.GetFileAsync(audioFilePath);
            var PlayMusic = new MediaElement();
            PlayMusic.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.Media;
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            PlayMusic.Play();
        }

        public void Pause()
        {
            if (mediaPlayer != null && mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
            {
                mediaPlayer.Pause();
            }
        }

        public void Stop()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Pause();
                mediaPlayer.Source = null;
                mediaPlayer = null;
            }
        }
    }
}
