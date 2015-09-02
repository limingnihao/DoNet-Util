using CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.Limingnihao.Api.Util.Util
{
    public class VolumeUtil
    {

        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

        /// <summary>
        /// 增加音量
        /// </summary>
        public static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 减小音量
        /// </summary>
        public static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 是否静音静止音量
        /// </summary>
        public static void Mute(bool value)
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            device.AudioEndpointVolume.Mute = value;
        }

        /// <summary>
        /// 设备系统音量
        /// </summary>
        /// <param name="arg">音量百分比</param>
        public static void SetVolume(float arg)
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            device.AudioEndpointVolume.Mute = false;
            device.AudioEndpointVolume.MasterVolumeLevelScalar = arg;
        }

        /// <summary>
        /// 获取当前系统音量
        /// </summary>
        /// <returns>音量百分比</returns>
        public static float GetVolume()
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            return device.AudioEndpointVolume.MasterVolumeLevelScalar;
        }
    }
}
