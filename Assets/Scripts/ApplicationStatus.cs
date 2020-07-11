using UnityEngine;

namespace Coderman
{
    public class ApplicationStatus : MonoBehaviour
    {
        public static bool IsQuitting = false;

        #region Unity Methods

        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }

        #endregion
    }
}
