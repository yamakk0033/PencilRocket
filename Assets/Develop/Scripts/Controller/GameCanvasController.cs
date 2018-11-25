using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject PanelObject;

        public void ChangeMode(GameMode.eMode mode)
        {
            switch (mode)
            {
                case GameMode.eMode.Pause:
                    PanelObject.SetActive(true);
                    break;
                default:
                    PanelObject.SetActive(false);
                    break;
            }
        }
    }
}
