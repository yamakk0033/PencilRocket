using Assets.Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvasManagerObject;
        [SerializeField] private GameObject blockGeneratorPrefab;
        [SerializeField] private GameObject playerObject;
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
            canvasManager = canvasManagerObject.GetComponent<CanvasManager>();

            var blockGo = Instantiate(blockGeneratorPrefab);
            blockGenerator = blockGo.GetComponent<BlockGenerator>();

            playerController = playerObject.GetComponent<PlayerController>();
        }

        private void Start()
        {
            Mode = GameMode.eMode.Title;
        }

        private IEnumerator Loop()
        {
            Mode = GameMode.eMode.Game;
            while (true)
            {
                if (blockGenerator.IsNeedleCollision)
                {
                    Mode = GameMode.eMode.GameOver;
                    break;
                }
                if (playerController.transform.position.y >= 8.0f)
                {
                    blockGenerator.SetPosition(Vector3.down * 3.0f);
                    playerController.SetPosition(Vector3.down * 3.0f);
                }

                yield return null;
            }

            yield break;
        }



        public void OnClickTitleLetter()
        {
            playerController.Init();
            blockGenerator.Init();
            StartCoroutine(Loop());
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
    }
}
