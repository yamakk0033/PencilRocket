using Assets.Constants;
using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class BlockController : MonoBehaviour
    {
        public bool IsBlockCollision { private set; get; } = false;
        public bool IsNeedleCollision => children.IsTouch;


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
            behaviour.Init(transform, rb, velocity);
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



        public void Init(float x, float y)
        {
            children.Init();
            IsBlockCollision = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            if (ScoreManager.Altitude < 10.0m)
            {
                pattern = BlockBehaviour.ePattern.No1;
            }
            else if (ScoreManager.Altitude < 20.0m)
            {
                pattern = (BlockBehaviour.ePattern)Random.Range(0, (int)BlockBehaviour.ePattern.No3 + 1);
            }
            else
            {
                pattern = (BlockBehaviour.ePattern)Random.Range(0, (int)BlockBehaviour.ePattern.No4 + 1);
            }

            transform.position = new Vector3(x, y);
            behaviour.InitProc(pattern);
        }

        public BlockBehaviour.eDirection GetDirection()
        {
            return behaviour.GetDirection();
        }
    }
}
