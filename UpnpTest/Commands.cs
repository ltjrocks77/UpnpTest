using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpTest
{
    class Commands
    {
        public enum CommandCodeList
        {
            PowerOff = 1,
            //2-7 Cant Tell What function
            // 8-11 Does something but only with TV
            //12-19 Cant tell
            EnterKey = 20,
            HomeMenu = 21,
            SettingsMenu = 22,
            BackKey = 23,
            VolumeUp = 24,
            VolumeDown = 25,
            Mute = 26,
            ChannelUp = 27,
            ChannelDown = 28,

            ThreeDMenu = 400,
            PreviousChannel = 403,
            ExitKey = 412,
            MyAppsMenu = 417,

            KEY_IDX_BLUE = 29,
            KEY_IDX_BTN_1 = 5,
            KEY_IDX_BTN_2 = 6,
            KEY_IDX_BTN_3 = 7,
            KEY_IDX_BTN_4 = 8,
            KEY_IDX_EXTERNAL_INPUT = 47,
            KEY_IDX_GREEN = 30,
            KEY_IDX_NETCAST = 408,
            KEY_IDX_PAUSE = 34,
            KEY_IDX_PLAY = 33,
            KEY_IDX_RED = 31,
            KEY_IDX_STOP = 35,
            KEY_IDX_YELLOW = 32,
        }
    }
}
