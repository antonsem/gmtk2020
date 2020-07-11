using UnityEngine;

namespace Coderman
{
    public class HelpPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        #region Unity Methods

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.F1)) return;

            canvas.enabled = !canvas.enabled;
            Events.Instance.helpStatus?.Invoke(canvas.enabled);
        }

        #endregion
    }
}