using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    [DisallowMultipleComponent]
    public class FirstScene : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}
