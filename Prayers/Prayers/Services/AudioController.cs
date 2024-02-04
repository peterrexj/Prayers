﻿using Prayers.ViewModels.Extras;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task PlayAudio(string audioFile)
        {
            await audioPlayerService.Play(audioFile);
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