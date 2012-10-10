using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Media;

namespace XBSlink.Android.Managers
{
    public class AudioManager
    {
          
        MediaPlayer _player;
        Activity _actualActivity;

        MediaRecorder _recorder;
        string path = "/sdcard/test.3gpp";

        void Record()
        {
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(path);
            _recorder.Prepare();
            _recorder.Start();
        }

        void RecordStop()
        {
            _recorder.Stop();
            _recorder.Reset();
        }

        void RecordPlay()
        {
            _player.SetDataSource(path);
            _player.Prepare();
            _player.Start();
        }

        public AudioManager(Activity actualActivity)
        {
            _actualActivity = actualActivity;
        }

        //public void PlaySound( int ResourceId) {

        //    PlaySound(_actualActivity, ResourceId);
        //}

        public void PlaySound(Activity ThisActivity, int ResourceId)
        {

            if (_player != null)
            {
                if (_player.IsPlaying)
                    _player.Stop();

            }
            _player = MediaPlayer.Create(ThisActivity, ResourceId);
            _player.Start();
        }

        public void PlayLeftNode(Activity ThisActivity)
        {
            PlaySound(ThisActivity,Resource.Raw.new_node);
        }

        public void PlayNewNode(Activity ThisActivity)
        {
            PlaySound(ThisActivity, Resource.Raw.node_left);
        }

        public void PlayIncommingChatMsg(Activity ThisActivity)
        {
            PlaySound(ThisActivity, Resource.Raw.incoming_chat_msg);
        }


    }
}
