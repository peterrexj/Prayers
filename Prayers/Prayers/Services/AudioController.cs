using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prayers.Services
{
    public class AudioController
    {
        public static bool IsPlaying = false;

        readonly IAudioPlayerService audioPlayerService;
        //readonly AudioPlayer01Service audioPlyService;

        public AudioController()
        {
            audioPlayerService = DependencyService.Get<IAudioPlayerService>();
            //audioPlyService = new AudioPlayer01Service();
        }

        public async Task PlayAudio(List<string> audioFiles)
        {
            //await audioPlyService.Play(audioFiles);
            await audioPlayerService.Play(audioFiles);
            IsPlaying = true;
        }

        public void StopAudio()
        {
            IsPlaying = false;
            //audioPlyService.Stop();
            audioPlayerService.Stop();
        }

        public void PauseAudio()
        {
            //audioPlyService.Pause();
            audioPlayerService.Pause();
        }

        public void AudioComplete()
        {
            IsPlaying = false;
        }
    }
}
