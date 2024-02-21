using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prayers.Services
{
    public class AudioController
    {
        public static bool IsPlaying = false;

        readonly IAudioPlayerService audioPlayerService;

        public AudioController()
        {
            audioPlayerService = DependencyService.Get<IAudioPlayerService>();
        }

        public async Task PlayAudio(List<string> audioFiles)
        {
            await audioPlayerService.Play(audioFiles);
            IsPlaying = true;
        }

        public void StopAudio()
        {
            IsPlaying = false;
            audioPlayerService.Stop();
        }

        public void PauseAudio()
        {
            audioPlayerService.Pause();
        }

        public void AudioComplete()
        {
            IsPlaying = false;
        }
    }
}
