using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void ChangeMode(GameManager.eMode mode)
    {
        switch (mode)
        {
            case GameManager.eMode.Pause:
                panel.SetActive(true);
                break;
            default:
                panel.SetActive(false);
                break;
        }
    }
}
