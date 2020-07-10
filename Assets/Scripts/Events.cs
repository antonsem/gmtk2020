using System;
using ExtraTools;
using UnityEngine;

namespace Coderman
{
    public class Events : Singleton<Events>
    {
        public Action<KeyCode> pressedKeyboardKey;
    }
}
