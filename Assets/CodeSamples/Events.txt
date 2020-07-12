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
        public Action<PopUpInfo> pushNotification;
        public Action<string, bool> popUpAcceptStatus;
        public Action<string> doneWithCode;
        public Action beatTheGame;
        public Action gameOver;
        public Action<bool> startCareer;
        public Action<bool> careerStatus;
        public Action playCloseNotificationSound;
        public Action playNotificationSound;
        public Action playPopUpSound;
    }
}