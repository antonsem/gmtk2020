using System;
using UnityEngine;

namespace Coderman
{
    [Serializable]
    public struct PopUpInfo
    {
        public string id;
        public string header;
        public PopUpAction[] actions;
        public float acceptTimer;
        public float closeTimer;
    }

    [Serializable]
    public struct PopUpAction
    {
        [TextArea(5, 25)]
        public string text;
        public KeyCode[] confirm;
        public KeyCode[] deny;
    }
}