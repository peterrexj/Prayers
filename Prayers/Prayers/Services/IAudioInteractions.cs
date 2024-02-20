using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prayers.Services
{
    public interface IAudioPlayerService
    {
        Task Play(List<string> pathToAudio);
        void Pause();
        void Stop();
    }
}
