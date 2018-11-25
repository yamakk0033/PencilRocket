using UnityEngine;
using UnityEngine.UI;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class AltitudeTextController : MonoBehaviour
    {
        private Text text;
        private string format = "#,##0.0m";

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            text.text = (ScoreManager.Altitude * 10.0m).ToString(format);
        }

        private void Update()
        {
            text.text = (ScoreManager.Altitude * 10.0m).ToString(format);
        }
    }
}
