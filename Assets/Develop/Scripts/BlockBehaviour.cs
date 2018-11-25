using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class BlockBehaviour
    {
        public enum ePattern
        {
            /// <summary>
            /// 一定の速度で移動
            /// </summary>
            No1 = 0,
            /// <summary>
            /// 徐々に速度が速くなる
            /// </summary>
            No2,
            /// <summary>
            /// 出現位置に距離がある
            /// </summary>
            No3,
            /// <summary>
            /// 出現位置が近い
            /// </summary>
            No4,
            /// <summary>
            /// 見えたところで一度止まり、再び動き出す。
            /// </summary>
            No5,
        }

        public enum eDirection
        {
            Left,
            Right,
        }

        private Dictionary<ePattern, Action> initDictionary = new Dictionary<ePattern, Action>();
        private Dictionary<ePattern, Action> updateDictionary = new Dictionary<ePattern, Action>();

        private Transform tran;
        private Rigidbody2D rb;
        private Vector2 velocity;
        private eDirection direction;
        private float stopTime = 0.0f;


        public BlockBehaviour()
        {
            initDictionary.Add(ePattern.No1, InitNo1);
            initDictionary.Add(ePattern.No2, InitNo1);
            initDictionary.Add(ePattern.No3, InitNo3);
            initDictionary.Add(ePattern.No4, InitNo4);
            initDictionary.Add(ePattern.No5, InitNo1);

            updateDictionary.Add(ePattern.No1, UpdateNo1);
            updateDictionary.Add(ePattern.No2, UpdateNo2);
            updateDictionary.Add(ePattern.No3, UpdateNo1);
            updateDictionary.Add(ePattern.No4, UpdateNo1);
            updateDictionary.Add(ePattern.No5, UpdateNo5);
        }



        private void InitNo1()
        {
            velocity = new Vector2(4.0f, 0.0f);
        }

        private void InitNo3()
        {
            tran.position = new Vector3(tran.position.x + Random.Range(1.0f, 3.0f), tran.position.y);
            velocity = new Vector2(4.0f, 0.0f);
        }

        private void InitNo4()
        {
            tran.position = new Vector3(tran.position.x - Random.Range(0.5f, 1.5f), tran.position.y);
            velocity = new Vector2(4.0f, 0.0f);
        }


        private void UpdateNo1()
        {
            rb.velocity = velocity;
        }

        private void UpdateNo2()
        {
            rb.velocity = velocity * 1.0001f;
        }

        private void UpdateNo5()
        {
            float x = tran.position.x;
            if (x < 0) x *= -1;


            if (x <= 14.0f && stopTime <= 1.0f)
            {
                stopTime += Time.fixedDeltaTime;
                rb.velocity = new Vector2(0.0f, 0.0f);
            }
            else
            {
                rb.velocity = velocity;
            }
        }



        public void Init(Transform t, Rigidbody2D r, Vector2 vel)
        {
            tran = t;
            rb = r;
            velocity = vel;
        }

        public void InitProc(ePattern ptn)
        {
            initDictionary[ptn]();

            direction = Random.Range(0, 2) == 0 ? eDirection.Left : eDirection.Right;
            stopTime = 0.0f;

            switch (direction)
            {
                case eDirection.Left:
                    tran.position = new Vector3(tran.position.x * -1, tran.position.y);
                    tran.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    break;
                case eDirection.Right:
                    tran.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    velocity *= -1;
                    break;
            }
        }

        public void UpdateProc(ePattern ptn)
        {
            updateDictionary[ptn]();
        }
    }
}
