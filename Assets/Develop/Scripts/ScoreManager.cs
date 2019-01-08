using UnityEngine;

namespace Assets
{
    public static class ScoreManager
    {
        /// <summary>
        /// 高度
        /// </summary>
        public static decimal Altitude { get; set; }

        /// <summary>
        /// 高度(表示用)
        /// </summary>
        public static decimal ViewAltitude { get { return Altitude * 10; } }
    }
}
