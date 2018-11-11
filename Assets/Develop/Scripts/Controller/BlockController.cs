using Assets.Constants;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public bool IsBlockCollision { private set; get; } = false;
    public bool IsNeedleCollision => children.IsTouch;


    public enum eDirection
    {
        Left,
        Right,
    }

    //private BlockGenerator parent;
    private NeedleController children;
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;

    private eDirection _direction;
    private eDirection direction
    {
        get { return _direction; }
        set
        {
            _direction = value;

            switch (value)
            {
                case eDirection.Left:
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    velocity = new Vector2(4.0f, 0.0f);
                    break;
                case eDirection.Right:
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    velocity = new Vector2(-4.0f, 0.0f);
                    break;
            }
        }
    }




    public void Init(eDirection dir)
    {
        children.Init();
        IsBlockCollision = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        direction = dir;

        //rb.bodyType = RigidbodyType2D.Dynamic;
    }



    private void Awake()
    {
        //parent = GetComponentInParent<BlockGenerator>();
        children = GetComponentInChildren<NeedleController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(IsNeedleCollision)
        {
            ChangeState();
        }
    }

    private void FixedUpdate()
    {
        if(rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.velocity = velocity;
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
}
