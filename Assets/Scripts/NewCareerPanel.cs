using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coderman
{
    public class NewCareerPanel : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F4))
            {
                Events.Instance.careerStatus?.Invoke(true);
                canvas.enabled = true;
            }

            if (!canvas.enabled) return;

            if (Input.GetKeyDown(KeyCode.Y))
            {
                ApplicationStatus.IsQuitting = true;
                SceneManager.LoadScene(0);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Events.Instance.careerStatus?.Invoke(false);
                canvas.enabled = false;
            }
        }
    }
}