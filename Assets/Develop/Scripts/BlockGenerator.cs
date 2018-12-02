using Assets.Controller;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [DisallowMultipleComponent]
    public class BlockGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject BlockPrefab = null;


        public bool IsNeedleCollision { get { return (target == null) ? false : target.IsNeedleCollision; } }

        private readonly int BLOCK_LIST_MAX = 10;

        private Queue<BlockController> blockQueue = new Queue<BlockController>();
        private BlockController target;
        private Vector3 firstPosition;
        private float colliderSizeY;


        private void Awake()
        {
            firstPosition = BlockPrefab.transform.position;

            var col = BlockPrefab.GetComponent<BoxCollider2D>();
            colliderSizeY = col.size.y * BlockPrefab.transform.localScale.y;

            foreach (int i in Enumerable.Range(0, BLOCK_LIST_MAX))
            {
                var go = Instantiate(BlockPrefab, transform);
                var bc = go.GetComponent<BlockController>();

                go.transform.parent = transform;
                go.SetActive(false);

                blockQueue.Enqueue(bc);
            }
        }

        private IEnumerator Loop()
        {
            while (true)
            {
                if (target.IsNeedleCollision)
                {
                    yield break;
                }
                else if (target.IsBlockCollision)
                {
                    this.Apper(target.transform.position.y + colliderSizeY);
                }

                yield return null;
            }
        }

        private void Apper(float posY)
        {
            if (target != null) blockQueue.Enqueue(target);

            target = blockQueue.Dequeue();
            target.Init(firstPosition.x, posY);

            target.gameObject.SetActive(true);
        }


        public void SetPosition(Vector3 pos)
        {
            target.transform.position += pos;
            foreach (var item in blockQueue)
            {
                item.transform.position += pos;
            }
        }

        public void Init()
        {
            if (target != null) target.gameObject.SetActive(false);
            foreach (var item in blockQueue)
            {
                item.gameObject.SetActive(false);
            }

            this.Apper(firstPosition.y);
            StartCoroutine(Loop());
        }

        public BlockController GetTarget()
        {
            return target;
        }
    }
}
