using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public bool IsNeedleCollision { get { return (target == null) ? false : target.IsNeedleCollision; } }

    private readonly int BLOCK_LIST_MAX = 10;

    [SerializeField] private GameObject blockPrefab = null;

    private Queue<BlockController> blockQueue = new Queue<BlockController>();
    private BlockController target = null;
    private Vector3 firstPosition = Vector3.zero;
    private float colliderSizeY;


    private void Awake()
    {
        firstPosition = blockPrefab.transform.position;

        var col = blockPrefab.GetComponent<BoxCollider2D>();
        colliderSizeY = col.size.y * blockPrefab.transform.localScale.y;

        foreach (int i in Enumerable.Range(0, BLOCK_LIST_MAX))
        {
            var go = Instantiate(blockPrefab, transform);
            var bc = go.GetComponent<BlockController>();

            go.transform.parent = transform;
            go.SetActive(false);

            blockQueue.Enqueue(bc);
        }
    }

    private void Start()
    {
        Init();
    }

    private IEnumerator Loop()
    {
        while(true)
        {
            if (target.IsBlockCollision)
            {
                var last_target = target;
                blockQueue.Enqueue(target);

                this.Apper(last_target.transform.position.y + colliderSizeY);
            }
            else if (target.IsNeedleCollision)
            {
                yield break;
            }

            yield return null;
        }
    }


    private void Apper(float pos_y)
    {
        var dir = Random.Range(0, 2) == 0 ? BlockController.eDirection.Left : BlockController.eDirection.Right;

        target = blockQueue.Dequeue();
        target.Init(dir);
        target.transform.position =
            new Vector3(
                (dir == BlockController.eDirection.Left) ? -firstPosition.x : firstPosition.x
                , pos_y
            );


        target.gameObject.SetActive(true);
    }


    public void SetPosition(Vector3 pos)
    {
        target.transform.position += pos;
        foreach(var item in blockQueue)
        {
            item.transform.position += pos;
        }
    }

    public void Init()
    {
        foreach (var item in blockQueue)
        {
            item.gameObject.SetActive(false);
        }

        this.Apper(firstPosition.y);
        StartCoroutine(Loop());
    }
}
