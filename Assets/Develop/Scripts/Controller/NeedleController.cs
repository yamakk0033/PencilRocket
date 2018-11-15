using Assets.Constants;
using UnityEngine;

namespace Assets.Controller
{
    public class NeedleController : MonoBehaviour
    {
        public bool IsTouch { private set; get; } = false;



        public void Init()
        {
            IsTouch = false;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagName.PLAYER)
            {
                IsTouch = true;
            }
        }
    }
}
