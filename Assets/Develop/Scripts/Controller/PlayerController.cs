using Assets;
using Assets.Constants;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float jumpTime = 0.0f;
    private bool isJump = false;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (TouchInput.GetLayerNo() == LayerNo.UI) return;


        if (TouchInput.GetState() == TouchInput.State.Began)
        {
            if (isJump)
            {
                rb.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
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
}
