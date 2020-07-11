using UnityEngine;

namespace Coderman
{
    public class CreditsPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        #region Unity Methods

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.F9)) return;

            canvas.enabled = !canvas.enabled;
            Events.Instance.creditsStatus?.Invoke(canvas.enabled);
        }

        #endregion
    }
}
