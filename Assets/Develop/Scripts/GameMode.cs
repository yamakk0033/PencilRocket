using UnityEngine;

namespace Assets
{
    /// <summary>
    /// ゲームモード(画面の状態)
    /// </summary>
    public class GameMode
    {
        public enum eMode
        {
            /// <summary>
            /// None
            /// </summary>
            None,
            /// <summary>
            /// タイトル
            /// </summary>
            Title,
            /// <summary>
            /// ゲーム
            /// </summary>
            Game,
            /// <summary>
            /// チュートリアル
            /// </summary>
            Tutorial,
            /// <summary>
            /// 一時停止(ポーズ)
            /// </summary>
            Pause,
            /// <summary>
            /// ゲームオーバー
            /// </summary>
            GameOver,
            /// <summary>
            /// クレジット(使用しているAssetなどを表示)
            /// </summary>
            Credits,
        }
    }
}
