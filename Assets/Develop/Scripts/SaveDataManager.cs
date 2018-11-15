using System;
using UnityEngine;

namespace Assets
{
    public static class SaveDataManager
    {
        [Serializable]
        public struct SaveData
        {
            public long MaxAltitude; // 標高
        }


        private static string Key { get; } = "001";


        public static SaveData Get()
        {
            return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(Key));
        }

        public static void Set(SaveData inst)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(inst));
        }
    }
}
