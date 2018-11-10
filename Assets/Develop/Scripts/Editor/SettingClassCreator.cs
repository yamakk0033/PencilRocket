using Assets.Editor.Constants;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>
    /// タグ、レイヤー、シーン、ソーティングレイヤー名を定数で管理するクラスを自動で作成するスクリプト
    /// </summary>
    public class SettingClassCreator : AssetPostprocessorEx
    {
        //変更を監視するディレクトリ名
        private const string TARGET_DIRECTORY_NAME = "ProjectSettings";

        //コマンド名
        private const string COMMAND_NAME = "Tools/Create/Setting Class";




        //ProjectSettings以下の設定が編集されたら自動で各スクリプトを作成
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var assetsList = new List<string[]>()
            {
               importedAssets
            };

            var targetDirectoryNameList = new List<string>()
            {
                TARGET_DIRECTORY_NAME
            };

            if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList))
            {
                Create();
            }
        }



        //スクリプトを作成します
        [MenuItem(COMMAND_NAME)]
        private static void Create()
        {
            //タグ
            Dictionary<string, string> tagDic = InternalEditorUtility.tags.ToDictionary(value => value);
            ConstantsClassCreator.Create("TagName", "タグ名を定数で管理するクラス", tagDic);


            //シーン
            var scenesNameDic = new Dictionary<string, string>();
            var scenesNoDic = new Dictionary<string, int>();

            foreach (int i in Enumerable.Range(0, EditorBuildSettings.scenes.Length))
            {
                string sceneName = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                scenesNameDic[sceneName] = sceneName;
                scenesNoDic[sceneName] = i;
            }

            CreateSceneData(scenesNoDic);

            ConstantsClassCreator.Create("SceneName", "シーン名を定数で管理するクラス", scenesNameDic);
            ConstantsClassCreator.Create("SceneNo", "シーン番号を定数で管理するクラス", scenesNoDic);


            //レイヤーとレイヤーマスク
            var layerNoDic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => LayerMask.NameToLayer(layer));
            var layerMaskNoDic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => 1 << LayerMask.NameToLayer(layer));

            ConstantsClassCreator.Create("LayerNo", "レイヤー番号を定数で管理するクラス", layerNoDic);
            ConstantsClassCreator.Create("LayerMaskNo", "レイヤーマスク番号を定数で管理するクラス", layerMaskNoDic);


            //ソーティングレイヤー
            Dictionary<string, string> sortingLayerDic = GetSortingLayerNames().ToDictionary(value => value);
            ConstantsClassCreator.Create("SortingLayerName", "ソーティングレイヤー名を定数で管理するクラス", sortingLayerDic);
        }



        //シーン名とシーン番号を関連付けたSceneDataを書き出す
        private static void CreateSceneData(Dictionary<string, int> scenesNoDic)
        {
            string dataPath = DirectoryPath.TOP_RESOURCES + FilePath.SCENE_DATA + Extension.ASSET;

            //SceneDataを新規作成、既にあるものは削除
            SceneData data = ScriptableObject.CreateInstance<SceneData>();
            AssetDatabase.DeleteAsset(dataPath);
            AssetDatabase.CreateAsset((ScriptableObject)data, dataPath);

            data.hideFlags = HideFlags.NotEditable;

            foreach (var valuePair in scenesNoDic)
            {
                var param = new SceneData.Param();
                param.Name = valuePair.Key;
                param.No = valuePair.Value;

                data.list.Add(param);
            }

            //変更を反映
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath(dataPath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty(obj);
        }



        //sortinglayerの名前一覧を取得
        private static string[] GetSortingLayerNames()
        {
            var internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }
    }
}
