using Assets.Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject CanvasManagerObject;
        [SerializeField] private GameObject BlockGeneratorPrefab;
        [SerializeField] private GameObject PlayerObject;
        private CanvasManager canvasManager;
        private BlockGenerator blockGenerator;
        private PlayerController playerController;

        private GameMode.eMode _mode;
        private GameMode.eMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode == value) return;

                _mode = value;
                canvasManager.ChangeMode(value);
            }
        }


        private void Awake()
        {
            canvasManager = CanvasManagerObject.GetComponent<CanvasManager>();

            var blockGo = Instantiate(BlockGeneratorPrefab);
            blockGenerator = blockGo.GetComponent<BlockGenerator>();

            playerController = PlayerObject.GetComponent<PlayerController>();

            Mode = GameMode.eMode.Title;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus && Mode == GameMode.eMode.Game)
            {
                Mode = GameMode.eMode.Pause;
                Time.timeScale = 0.0f;
            }
        }

        private IEnumerator GameLoop()
        {
            Mode = GameMode.eMode.Game;
            while (true)
            {
                if (blockGenerator.IsNeedleCollision)
                {
                    Mode = GameMode.eMode.GameOver;
                    break;
                }
                if (playerController.transform.position.y >= 15.0f)
                {
                    blockGenerator.SetPosition(Vector3.down * 10.0f);
                    playerController.SetPosition(Vector3.down * 10.0f);
                }

                yield return null;
            }

            yield break;
        }



        public void OnClickTitleLetter()
        {
            playerController.Init();
            blockGenerator.Init();
            StartCoroutine(GameLoop());
        }

        public void OnClickPauseButton()
        {
            if (Mode == GameMode.eMode.Pause)
            {
                Time.timeScale = 1.0f;
                Mode = GameMode.eMode.Game;
            }
            else
            {
                Time.timeScale = 0.0f;
                Mode = GameMode.eMode.Pause;
            }
        }

        public void OnClickAdsButton()
        {
            Advertisement.Show();
        }

        public void OnClickCredisButton()
        {
            Mode = (Mode == GameMode.eMode.GameOver) ? GameMode.eMode.Credits : GameMode.eMode.GameOver;
        }
    }
}
