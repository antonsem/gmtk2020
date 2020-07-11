using UnityEditor;
using UnityEngine;

namespace Coderman
{
    public class QuitPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        #region Unity Methods

        private void Update()
        {
            if (canvas.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    canvas.enabled = false;
                    Events.Instance.exitStatus?.Invoke(canvas.enabled);
                    return;
                }
            }

            if (!Input.GetKeyDown(KeyCode.F10)) return;

            canvas.enabled = !canvas.enabled;
            Events.Instance.exitStatus?.Invoke(canvas.enabled);
        }

        #endregion
    }
}