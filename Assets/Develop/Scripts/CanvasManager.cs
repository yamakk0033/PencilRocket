using Assets.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [DisallowMultipleComponent]
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject titleCanvas;
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject clearCanvas;
        [SerializeField] private GameObject gameOverCanvas;
        private GameCanvasController gameCanvasController;
        private Dictionary<GameObject, List<GameMode.eMode>> dictionary;

        private void Awake()
        {
            gameCanvasController = gameCanvas.GetComponent<GameCanvasController>();

            dictionary = new Dictionary<GameObject, List<GameMode.eMode>>()
            {
                { titleCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Title } },
                { gameCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Game, GameMode.eMode.Pause } },
                { clearCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Clear } },
                { gameOverCanvas, new List<GameMode.eMode>(){ GameMode.eMode.GameOver } },
            };
        }

        public void ChangeMode(GameMode.eMode mode)
        {
            foreach (var pair in dictionary)
            {
                pair.Key.SetActive(pair.Value.Any(v => v == mode));
            }

            gameCanvasController.ChangeMode(mode);
        }
    }
}
