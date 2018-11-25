using Assets.Constants;
using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class BlockController : MonoBehaviour
    {
        public bool IsBlockCollision { private set; get; } = false;
        public bool IsNeedleCollision => children.IsTouch;

        public enum eDirection
        {
            Left,
            Right,
        }


        private NeedleController children;
        private Rigidbody2D rb;
        private Vector2 velocity = Vector2.zero;
        private BlockBehaviour behaviour;
        private BlockBehaviour.ePattern pattern;


        private void Awake()
        {
            children = GetComponentInChildren<NeedleController>();
            rb = GetComponent<Rigidbody2D>();

            behaviour = new BlockBehaviour();
            //behaviour.Init(transform, rb, velocity);
        }

        private void Update()
        {
            if (IsNeedleCollision)
            {
                ChangeState();
            }
        }

        private void FixedUpdate()
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                //rb.velocity = velocity;
                behaviour.UpdateProc(pattern);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagName.PLAYER)
            {
                ChangeState();
            }
        }

        private void ChangeState()
        {
            IsBlockCollision = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }



        public void Init(float x, float y, eDirection dir, BlockBehaviour.ePattern ptn)
        {
            children.Init();
            IsBlockCollision = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            pattern = ptn;
            behaviour.InitProc(ptn);

            switch (dir)
            {
                case eDirection.Left:
                    transform.position = new Vector3(-x, y);
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    velocity = new Vector2(4.0f, 0.0f);
                    break;
                case eDirection.Right:
                    transform.position = new Vector3(x, y);
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    velocity = new Vector2(-4.0f, 0.0f);
                    break;
            }
        }
    }
}
