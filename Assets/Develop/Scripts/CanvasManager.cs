using Assets.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject titleCanvas;
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject clearCanvas;
        [SerializeField] private GameObject gameOverCanvas;
        private GameCanvasController gameCanvasController;
        private Dictionary<GameObject, List<GameManager.eMode>> dictionary;

        private void Awake()
        {
            gameCanvasController = gameCanvas.GetComponent<GameCanvasController>();

            dictionary = new Dictionary<GameObject, List<GameManager.eMode>>()
        {
            { titleCanvas, new List<GameManager.eMode>(){ GameManager.eMode.Title } },
            { gameCanvas, new List<GameManager.eMode>(){ GameManager.eMode.Game, GameManager.eMode.Pause } },
            { clearCanvas, new List<GameManager.eMode>(){ GameManager.eMode.Clear } },
            { gameOverCanvas, new List<GameManager.eMode>(){ GameManager.eMode.GameOver } },
        };
        }

        public void ChangeMode(GameManager.eMode mode)
        {
            foreach (var pair in dictionary)
            {
                pair.Key.SetActive(pair.Value.Any(v => v == mode));
            }

            gameCanvasController.ChangeMode(mode);
        }
    }
}
