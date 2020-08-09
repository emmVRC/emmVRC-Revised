using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace emmVRC.Libraries
{
    class MediaControl
    {
        //This class is NOT quest compatible if ML is ever ported!
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        private static int KEYEVENTF_KEYUP = 0x0002;

        private static byte mediaPlayPause = (byte)Keys.MediaPlayPause;
        private static byte mediaNextTrack = (byte)Keys.MediaNextTrack;
        private static byte mediaPreviousTrack = (byte)Keys.MediaPreviousTrack;
        private static byte mediaStop = (byte)Keys.MediaStop;
        private static byte volUp = (byte)Keys.VolumeUp;
        private static byte volDown = (byte)Keys.VolumeDown;
        private static byte volMute = (byte)Keys.VolumeMute;

        internal static void PlayPause() {
            keybd_event(mediaPlayPause, mediaPlayPause, 0, 0);
            keybd_event(mediaPlayPause, mediaPlayPause, KEYEVENTF_KEYUP, 0);
        }
        internal static void PrevTrack() {
            keybd_event(mediaPreviousTrack, mediaPreviousTrack, 0, 0);
            keybd_event(mediaPreviousTrack, mediaPreviousTrack, KEYEVENTF_KEYUP, 0);
        }
        internal static void NextTrack() {
            keybd_event(mediaNextTrack, mediaNextTrack, 0, 0);
            keybd_event(mediaNextTrack, mediaNextTrack, KEYEVENTF_KEYUP, 0);
        }
        internal static void Stop() {
            keybd_event(mediaStop, mediaStop, 0, 0);
            keybd_event(mediaStop, mediaStop, KEYEVENTF_KEYUP, 0);
        }
        internal static void VolumeUp() {
            keybd_event(volUp, volUp, 0, 0);
            keybd_event(volUp, volUp, KEYEVENTF_KEYUP, 0);
        }
        internal static void VolumeDown() {
            keybd_event(volDown, volDown, 0, 0);
            keybd_event(volDown, volDown, KEYEVENTF_KEYUP, 0);
        }
        internal static void VolumeMute() {
            keybd_event(volMute, volMute, 0, 0);
            keybd_event(volMute, volMute, KEYEVENTF_KEYUP, 0);
        }
    }
}
