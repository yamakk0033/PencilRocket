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
        private bool isJump = false;


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
                if (isJump)
                {
                    rb.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                    audioSource.Play();
                }
            }
            else if (TouchInput.GetState() == TouchInput.State.Moved)
            {
                jumpTime += Time.deltaTime;
                if (isJump && jumpTime <= 0.3f)
                {
                    rb.velocity = Vector2.up * 6;
                }
            }
            else if (TouchInput.GetState() == TouchInput.State.Ended)
            {
                isJump = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagName.GROUND || collision.gameObject.tag == TagName.BLOCK)
            {
                jumpTime = 0.0f;
                isJump = true;
            }
        }



        public void SetPosition(Vector3 pos)
        {
            transform.position += pos;
        }

        public void Init()
        {
            transform.position = firstPosition;
            transform.rotation = firstRotation;
        }
    }
}
