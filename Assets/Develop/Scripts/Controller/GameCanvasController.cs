using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject PanelObject;
        [SerializeField] private GameObject PauseButton;

        public void ChangeMode(GameMode.eMode mode)
        {
            switch (mode)
            {
                case GameMode.eMode.Pause:
                    PanelObject.SetActive(true);
                    PauseButton.SetActive(true);
                    break;
                case GameMode.eMode.Tutorial:
                    PanelObject.SetActive(false);
                    PauseButton.SetActive(false);
                    break;
                default:
                    PanelObject.SetActive(false);
                    PauseButton.SetActive(true);
                    break;
            }
        }
    }
}
