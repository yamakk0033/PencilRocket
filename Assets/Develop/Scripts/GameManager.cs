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
        [SerializeField] private GameObject HandObject;
        private CanvasManager canvasManager;
        private BlockGenerator blockGenerator;
        private PlayerController playerController;
        private SaveDataManager.SaveData saveData;

        private GameMode.eMode _mode;
        private GameMode.eMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode == value) return;

                _mode = value;

                HandObject.SetActive(value == GameMode.eMode.Tutorial);
                canvasManager.ChangeMode(value);
            }
        }


        private void Awake()
        {
            canvasManager = CanvasManagerObject.GetComponent<CanvasManager>();

            var blockGo = Instantiate(BlockGeneratorPrefab);
            blockGenerator = blockGo.GetComponent<BlockGenerator>();

            playerController = PlayerObject.GetComponent<PlayerController>();
            saveData = SaveDataManager.Get();

            Mode = GameMode.eMode.Title;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus && Mode == GameMode.eMode.Game)
            {
                Time.timeScale = 0.0f;
                Mode = GameMode.eMode.Pause;
            }
        }

        private IEnumerator GameLoop()
        {
            Mode = GameMode.eMode.Game;
            while (true)
            {
                Tutorial();

                if(Mode == GameMode.eMode.Tutorial)
                {
                    if (TouchInput.GetState() == TouchInput.State.Began)
                    {
                        Time.timeScale = 1.0f;
                        Mode = GameMode.eMode.Game;
                    }
                }

                if (playerController.transform.position.y >= 15.0f)
                {
                    blockGenerator.SetPosition(Vector3.down * 10.0f);
                    playerController.SetPosition(Vector3.down * 10.0f);
                }
                if (blockGenerator.IsNeedleCollision)
                {
                    saveData.IsTutorial = false;
                    SaveDataManager.Set(saveData);
                    Mode = GameMode.eMode.GameOver;
                    break;
                }

                yield return null;
            }

            yield break;
        }

        private void Tutorial()
        {
            if (!saveData.IsTutorial) return;
            if (Mode != GameMode.eMode.Game) return;

            var target = blockGenerator.GetTarget();
            float tx = target.transform.position.x;
            tx *= (target.GetDirection() == BlockBehaviour.eDirection.Left) ? -1 : 1;

            if (!playerController.IsJumping() &&
                tx <= playerController.transform.position.x + 13.0f &&
                ScoreManager.Altitude <= 2.0m)
            {
                Time.timeScale = 0.0f;
                Mode = GameMode.eMode.Tutorial;
            }
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
