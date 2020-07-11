using System;
using UnityEngine;

namespace Coderman
{
    public class Events : Singleton<Events>
    {
        public Action<KeyCode> pressedKeyboardKey;
        public Action<bool> popUpStatus;
        public Action<bool> helpStatus;
        public Action<bool> creditsStatus;
        public Action<bool> exitStatus;
        public Action<PopUpInfo> setPopUp;
        public Action<string, bool> popUpAcceptStatus;
    }
}
