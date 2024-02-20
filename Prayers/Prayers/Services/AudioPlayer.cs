//using Microsoft.AppCenter.Crashes;
//using Pj.Library;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Prayers.Services
//{
//    public class AudioPlayer01Service
//    {
//        private Queue<string> _mediaFilesToPlay;

//        public AudioPlayer01Service()
//        {
//            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.PlaybackEnded += Current_PlaybackEnded;
//        }

//        private void Current_PlaybackEnded(object sender, EventArgs e)
//        {
//            try
//            {
//                if (_mediaFilesToPlay.Count > 0)
//                {
//                    CheckAudioToPlay();
//                }
//                else
//                {
//                    _mediaFilesToPlay = null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//            }
//        }

//        private void CheckAudioToPlay()
//        {
//            if (_mediaFilesToPlay.Count <= 0)
//            {
//                SharedServices.AudioController.AudioComplete();
//                return;
//            }
//            var file = _mediaFilesToPlay.Dequeue();
//            if (file.IsEmpty()) return;

//            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Load(file);
//            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
//        }

//        public async Task Play(List<string> audioFilePath)
//        {
//            try
//            {
//                await Task.Run(() =>
//                {
//                    //_mediaFilesToPlay.Count > 0 = is paused
//                    //_mediaFilesToPlay == null &&                     //duration != current position

//                    var same = Math.Round(Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Duration, 0) !=
//                        Math.Round(Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.CurrentPosition, 0);

//                    if (_mediaFilesToPlay?.Count > 0 || 
//                        (_mediaFilesToPlay == null && same))
//                    {

//                    //if (Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Duration > 0 &&
//                    //    (_mediaFilesToPlay == null || _mediaFilesToPlay.Count > 0))
//                    //{
//                        Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
//                        return;
//                    }
//                    //if (Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current != null && Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.IsPlaying == false)
//                    //{
//                    //    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
//                    //}
//                    else
//                    {
//                        _mediaFilesToPlay = new Queue<string>();
//                        audioFilePath.Iter(f => _mediaFilesToPlay.Enqueue(f));
//                        CheckAudioToPlay();
//                    }

//                    //if (Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current != null && Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.IsPlaying) return;
//                    //if (Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current != null && Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.IsPlaying == false)
//                    //{
//                    //    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
//                    //}
//                    //else
//                    //{
//                    //    _mediaFilesToPlay.Clear();
//                    //    audioFilePath.Iter(f => _mediaFilesToPlay.Enqueue(f));
//                    //    CheckAudioToPlay();
//                    //}
//                });

//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//            }
//        }

//        public void Pause()
//        {
//            try
//            {
//                if (!Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.IsPlaying) return;
//                Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Pause();
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//            }
//        }

//        public void Stop()
//        {
//            try
//            {
//                if (Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current == null) return;
//                Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Stop();
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//            }
//        }
//    }
//}
