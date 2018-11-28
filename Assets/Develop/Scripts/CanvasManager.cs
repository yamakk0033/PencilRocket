using Assets.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [DisallowMultipleComponent]
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject TitleCanvas;
        [SerializeField] private GameObject GameCanvas;
        [SerializeField] private GameObject ClearCanvas;
        [SerializeField] private GameObject GameOverCanvas;
        private GameCanvasController gameCanvasController;
        private GameOverCanvasController gameOverCanvasController;
        private Dictionary<GameObject, List<GameMode.eMode>> dictionary;

        private void Awake()
        {
            gameCanvasController = GameCanvas.GetComponent<GameCanvasController>();
            gameOverCanvasController = GameOverCanvas.GetComponent<GameOverCanvasController>();

            dictionary = new Dictionary<GameObject, List<GameMode.eMode>>()
            {
                { TitleCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Title } },
                { GameCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Game, GameMode.eMode.Pause } },
                { ClearCanvas, new List<GameMode.eMode>(){ GameMode.eMode.Clear } },
                { GameOverCanvas, new List<GameMode.eMode>(){ GameMode.eMode.GameOver, GameMode.eMode.Credits } },
            };
        }

        public void ChangeMode(GameMode.eMode mode)
        {
            foreach (var pair in dictionary)
            {
                pair.Key.SetActive(pair.Value.Any(v => v == mode));
            }

            gameCanvasController.ChangeMode(mode);
            gameOverCanvasController.ChangeMode(mode);
        }
    }
}
