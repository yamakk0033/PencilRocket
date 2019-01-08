using System;
using UnityEngine;

namespace Assets
{
    public static class SaveDataManager
    {
        [Serializable]
        public struct SaveData
        {
            /// <summary>
            /// True：チュートリアルを行う
            /// </summary>
            public bool IsTutorial;
        }


        private static string Key { get; } = "001";


        public static SaveData Get()
        {
            string value = PlayerPrefs.GetString(Key, "");
            if(string.IsNullOrWhiteSpace(value))
            {
                var sd = new SaveData();
                sd.IsTutorial = true;
                return sd;
            }

            return JsonUtility.FromJson<SaveData>(value);
        }

        public static void Set(SaveData inst)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(inst));
        }
    }
}
