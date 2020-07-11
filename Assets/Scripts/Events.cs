using System;
using UnityEngine;

namespace Coderman
{
    public class Events : Singleton<Events>
    {
        public Action<KeyCode> pressedKeyboardKey;
        public Action<bool> popUpStatus;
        public Action<PopUpInfo> setPopUp;
        public Action<string, bool> popUpAcceptStatus;
    }
}
