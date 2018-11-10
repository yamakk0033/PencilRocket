using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>
    /// �萔���Ǘ�����N���X�𐶐�����N���X
    /// </summary>
    public static class ConstantsClassCreator
    {
        //�����ȕ������Ǘ�����z��
        private static readonly string[] INVALUD_CHARS =
        {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

        //�萔�̋�؂蕶��
        private const char DELIMITER = '_';

        //�^��
        private const string STRING_NAME = "string";
        private const string INT_NAME = "int";
        private const string FLOAT_NAME = "float";

        /// <summary>
        /// �萔���Ǘ�����N���X��������������
        /// </summary>
        /// <param name="className">�N���X�̖��O</param>
        /// <param name="classInfo">�Ȃ�̃N���X����������R�����g��</param>
        /// <param name="variableDic">�萔���Ƃ��̒l���Z�b�g�œo�^����Dictionary</param>
        /// <typeparam name="T">�萔�̌^�Astring��int��float</typeparam>
        public static void Create<T>(string className, string classInfo, Dictionary<string, T> valueDic)
        {
            //���͂��ꂽ�^�̔���
            string typeName = null;

            if (typeof(T) == typeof(string))
            {
                typeName = STRING_NAME;
            }
            else if (typeof(T) == typeof(int))
            {
                typeName = INT_NAME;
            }
            else if (typeof(T) == typeof(float))
            {
                typeName = FLOAT_NAME;
            }
            else
            {
                Debug.Log(className + Assets.Editor.Constants.Extension.SCRIPT + "�̍쐬�Ɏ��s���܂���.�z��O�̌^" + typeof(T).Name + "�����͂���܂���");
                return;
            }


            //���͂��ꂽ������key���疳���ȕ�������폜���āA�啶����_��ݒ肵���萔���Ɠ������̂ɕύX���V���Ȏ����ɓo�^
            //���̒萔�̍ő咷���߂�Ƃ���ŁA_���܂߂����̂��擾�������̂ŁA��Ɏ��s
            var newValueDic = new Dictionary<string, T>();

            foreach (var valuePair in valueDic)
            {
                string newKey = RemoveInvalidChars(valuePair.Key);
                newKey = SetDelimiterBeforeUppercase(newKey);
                newValueDic[newKey] = valuePair.Value;
            }

            //�萔���̍ő咷���擾���A�󔒐�������
            int keyLengthMax = 1 + newValueDic.Keys.Select(key => key.Length).Max();

            //�R�����g���ƃN���X�������
            var builder = new StringBuilder();

            builder.AppendLine  ("namespace Assets.Constants");
            builder.AppendLine  ("{");
            builder.AppendLine  ("    /// <summary>");
            builder.AppendFormat("    /// {0}", classInfo).AppendLine();
            builder.AppendLine  ("    /// </summary>");
            builder.AppendFormat("    public static class {0}", className).AppendLine();
            builder.AppendLine  ("    {");

            //���͂��ꂽ�萔�Ƃ��̒l�̃y�A�������o���Ă���
            foreach (var valuePair in newValueDic)
            {
                if (string.IsNullOrEmpty(valuePair.Key))
                {
                    continue;
                }

                //�C�R�[�������ԗp�ɋ󔒂𒲐�����
                string EqualStr = string.Format("{0, " + (keyLengthMax - valuePair.Key.Length).ToString() + "}", "=");

                //��L�Ŕ��肵���^�ƒ萔�������
                builder.AppendFormat(@"        public const {0} {1} {2} ", typeName, valuePair.Key, EqualStr);

                //T��string�̏ꍇ�͒l�̑O���"��t����
                if (typeName == STRING_NAME)
                {
                    builder.AppendFormat(@"""{0}"";", valuePair.Value).AppendLine();
                }
                //T��float�̏ꍇ�͒l�̌��f��t����
                else if (typeName == FLOAT_NAME)
                {
                    builder.AppendFormat(@"{0}f;", valuePair.Value).AppendLine();
                }
                else
                {
                    builder.AppendFormat(@"{0};", valuePair.Value).AppendLine();
                }
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            //�����o���A�t�@�C�����̓N���X��.cs
            string exportPath = Assets.Editor.Constants.DirectoryPath.AUTO_CREATING_CONSTANTS + className + Assets.Editor.Constants.Extension.SCRIPT;

            //�����o����̃f�B���N�g����������΍쐬
            string directoryName = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.Log(className + Assets.Editor.Constants.Extension.SCRIPT + "create success");
        }


        /// <summary>
        /// �����ȕ������폜���܂�
        /// </summary>
        private static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
            return str;
        }

        /// <summary>
        /// ��؂蕶����啶���̑O�ɐݒ肷��
        /// </summary>
        private static string SetDelimiterBeforeUppercase(string str)
        {
            string conversionStr = "";


            for (int strNo = 0; strNo < str.Length; strNo++)
            {
                bool isSetDelimiter = true;

                //�ŏ��ɂ͐ݒ肵�Ȃ�
                if (strNo == 0)
                {
                    isSetDelimiter = false;
                }
                //�������������Ȃ�ݒ肵�Ȃ�
                else if (char.IsLower(str[strNo]) || char.IsNumber(str[strNo]))
                {
                    isSetDelimiter = false;
                }
                //���肵�Ă�̑O���啶���Ȃ�ݒ肵�Ȃ�(�A���啶���̎�)
                else if (char.IsUpper(str[strNo - 1]) && !char.IsNumber(str[strNo]))
                {
                    isSetDelimiter = false;
                }
                //���肵�Ă镶�������̕����̑O����؂蕶���Ȃ�ݒ肵�Ȃ�
                else if (str[strNo] == DELIMITER || str[strNo - 1] == DELIMITER)
                {
                    isSetDelimiter = false;
                }

                //�����ݒ�
                if (isSetDelimiter)
                {
                    conversionStr += DELIMITER.ToString();
                }
                conversionStr += str.ToUpper()[strNo];

            }

            return conversionStr;
        }

    }
}