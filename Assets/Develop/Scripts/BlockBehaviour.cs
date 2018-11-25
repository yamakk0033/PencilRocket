using Assets.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class BlockBehaviour : MonoBehaviour
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

        private Dictionary<ePattern, Action> initDictionary = new Dictionary<ePattern, Action>();
        private Dictionary<ePattern, Action> updateDictionary = new Dictionary<ePattern, Action>();

        private Transform tran;
        private Rigidbody2D rb;
        private Vector2 velocity;
        private float stopTime = 0.0f;


        private void Awake()
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



        public void Init(Transform t, Rigidbody2D r, Vector2 vel, BlockController.eDirection dir)
        {
            tran = t;
            rb = r;
            velocity = vel;
        }

        public void InitProc(ePattern ptn)
        {
            stopTime = 0.0f;
            initDictionary[ptn]();
        }

        public void UpdateProc(ePattern ptn)
        {
            updateDictionary[ptn]();
        }
    }
}
