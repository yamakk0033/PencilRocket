using Assets.Constants;
using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        private Vector3 firstPosition;
        private Quaternion firstRotation;
        private Rigidbody2D rb;
        private AudioSource audioSource;

        private float jumpTime = 0.0f;
        private bool isJumping = false;

        private float maxY = 0.0f;


        private void Awake()
        {
            firstPosition = transform.position;
            firstRotation = transform.rotation;
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (TouchInput.GetLayerNo() == LayerNo.UI) return;


            if (TouchInput.GetState() == TouchInput.State.Began)
            {
                if (!isJumping)
                {
                    isJumping = true;
                    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    audioSource.Play();
                }
            }
            else if (TouchInput.GetState() == TouchInput.State.Moved)
            {
                jumpTime += Time.deltaTime;
                if (isJumping && jumpTime <= 0.2f)
                {
                    rb.velocity = Vector2.up * 4;
                }
            }
            else if(TouchInput.GetState() == TouchInput.State.Ended)
            {
                jumpTime = 0.3f;
            }


            if(transform.position.y > maxY)
            {
                ScoreManager.Altitude += (decimal)(transform.position.y - maxY);
                maxY = transform.position.y;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagName.GROUND || collision.gameObject.tag == TagName.BLOCK)
            {
                jumpTime = 0.0f;
                isJumping = false;
            }
        }



        public void Init()
        {
            transform.position = firstPosition;
            transform.rotation = firstRotation;

            maxY = firstPosition.y;
            ScoreManager.Altitude = 0m;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position += pos;
            maxY += pos.y;
        }

        public bool IsJumping()
        {
            return isJumping;
        }
    }
}
