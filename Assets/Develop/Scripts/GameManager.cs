using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum eMode
    {
        None,
        Title,
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
            if (_mode == value) return;

            _mode = value;
            ChangeMode(value);
        }
    }

    [SerializeField] private GameObject canvasManagerObject;
    [SerializeField] private GameObject blockGeneratorPrefab;
    [SerializeField] private GameObject playerObject;
    private CanvasManager canvasManager;
    private BlockGenerator blockGenerator;
    private PlayerController playerController;

    private void Start()
    {
        canvasManager = canvasManagerObject.GetComponent<CanvasManager>();

        var blockGo = Instantiate(blockGeneratorPrefab);
        blockGenerator = blockGo.GetComponent<BlockGenerator>();
        blockGenerator.gameObject.SetActive(false);

        playerController = playerObject.GetComponent<PlayerController>();

        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        Mode = eMode.Title;
        while (Mode == eMode.Title) yield return null;

        Mode = eMode.Game;
        while(true)
        {
            if(blockGenerator.IsNeedleCollision)
            {
                Mode = eMode.GameOver;
                break;
            }
            if(playerController.transform.position.y >= 8.0f)
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
        canvasManager.ChangeMode(m);

        switch (m)
        {
            case eMode.Game:
                if(!blockGenerator.gameObject.activeSelf) blockGenerator.gameObject.SetActive(true);
                break;
            case eMode.GameOver:
                blockGenerator.gameObject.SetActive(false);
                break;
        }
    }



    public void OnClickTitleLetter()
    {
        if(Mode == eMode.Title)
        {
            Mode = eMode.Game;
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

    public void OnClickTitleBackButton()
    {
        StartCoroutine(Loop());
    }
}
