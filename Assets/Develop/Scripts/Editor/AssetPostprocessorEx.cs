using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets.Editor
{
    /// <summary>
    /// AssetPostprocessor�̊g���N���X
    /// </summary>
    public class AssetPostprocessorEx : AssetPostprocessor
    {
        /// <summary>
        /// ���͂��ꂽassets�̒��ɁA�f�B���N�g���̃p�X��directoryName�̕��͂��邩
        /// </summary>
        protected static bool ExistsDirectoryInAssets(List<string[]> assetsList, List<string> targetDirectoryNameList)
        {
            return assetsList
                .Any(assets => assets                                       //���͂��ꂽassetsList�Ɉȉ��̏����𖞂������v�f���܂܂�Ă��邩����
                 .Select(asset => System.IO.Path.GetDirectoryName(asset))   //assets�Ɋ܂܂�Ă���t�@�C���̃f�B���N�g�������������X�g�ɂ��Ď擾
                 .Intersect(targetDirectoryNameList)                         //��L�̃��X�g�Ɠ��͂��ꂽ�f�B���N�g�����̃��X�g�̈�v���Ă��镨�̃��X�g���擾
                 .Count() > 0);                                              //��v���Ă��镨�����邩
        }
    }
}
