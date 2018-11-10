using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum eMode
    {
        Title,
        Start,
        Game,
        Pause,
        Clear,
        GameOver,
    }

    private eMode _mode;
    private eMode Mode
    {
        get { return _mode; }
        set
        {
            _mode = value;
            ChangeMode(value);
        }
    }

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject clearCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject blockGeneratorPrefab;
    [SerializeField] private GameObject playerObject;
    private BlockGenerator blockGenerator;
    private PlayerController playerController;

    private void Start()
    {
        var blockGo = Instantiate(blockGeneratorPrefab);
        blockGenerator = blockGo.GetComponent<BlockGenerator>();
        blockGenerator.gameObject.SetActive(false);

        playerController = playerObject.GetComponent<PlayerController>();

        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        var wait = new WaitForSeconds(1.0f);

        Mode = eMode.Start;
        yield return wait;

        Mode = eMode.Game;
        blockGenerator.gameObject.SetActive(true);
        while(true)
        {
            if(blockGenerator.IsNeedleCollision)
            {
                Mode = eMode.GameOver;
                break;
            }
            if(playerObject.transform.position.y >= 8.0f)
            {
                blockGenerator.SetPosition(Vector3.down * 3.0f);
                playerController.SetPosition(Vector3.down * 3.0f);
            }

            yield return null;
        }


        yield break;
    }



    private void ChangeMode(eMode m)
    {
        new Dictionary<eMode, GameObject>()
            {
                { eMode.Title, titleCanvas.gameObject },
                { eMode.Game, gameCanvas.gameObject },
                { eMode.Pause, pauseCanvas.gameObject },
                { eMode.Clear, clearCanvas.gameObject },
                { eMode.GameOver, gameOverCanvas.gameObject },
            }
        .ToList().ForEach(pair => pair.Value.SetActive(pair.Key == m));


        switch (m)
        {
            case eMode.Game:
                blockGenerator.gameObject.SetActive(true);
                break;
            case eMode.GameOver:
                blockGenerator.gameObject.SetActive(false);
                break;
        }
    }



    public void OnClickPauseButton()
    {
        if(Mode == eMode.Pause)
        {
            Time.timeScale = 1.0f;
            Mode = eMode.Game;
        }
        else
        {
            Time.timeScale = 0.0f;
            Mode = eMode.Pause;
        }
    }
}
