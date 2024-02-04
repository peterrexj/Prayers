using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prayers.Services
{
    public interface IAudioPlayerService
    {
        Task Play(string pathToAudio);
        void Pause();
        void Stop();
    }
}
