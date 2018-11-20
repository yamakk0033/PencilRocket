using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void ChangeMode(GameMode.eMode mode)
        {
            switch (mode)
            {
                case GameMode.eMode.Pause:
                    panel.SetActive(true);
                    break;
                default:
                    panel.SetActive(false);
                    break;
            }
        }
    }
}
