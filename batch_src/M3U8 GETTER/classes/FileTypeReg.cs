using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{
    public class FileTypeRegInfo
    {
        /// <summary>
        /// 目标类型文件的扩展名
        /// </summary>
        public string ExtendName; 

        /// <summary>
        /// 目标文件类型说明
        /// </summary>
        public string Description;

        /// <summary>
        /// 目标类型文件关联的图标
        /// </summary>
        public string IcoPath;

        /// <summary>
        /// 打开目标类型文件的应用程序
        /// </summary>
        public string ExePath;

        public FileTypeRegInfo()
        {
        }

        public FileTypeRegInfo(string extendName)
        {
            this.ExtendName = extendName;
        }
    }

    /// <summary>
    /// FileTypeRegister 用于注册自定义的文件类型。
    /// zhuweisky 2005.08.31
    /// </summary>
    public class FileTypeRegister
    {
        #region RegisterFileType
        /// <summary>
        /// RegisterFileType 使文件类型与对应的图标及应用程序关联起来。
        /// </summary>        
        public static void RegisterFileType(FileTypeRegInfo regInfo)
        {
            if (FileTypeRegistered(regInfo.ExtendName))
            {
                return;
            }
            string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            fileTypeKey.SetValue("", relationName);
            fileTypeKey.Close();
            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", regInfo.Description);
            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.IcoPath);
            RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            RegistryKey openKey = shellKey.CreateSubKey("Open");
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", "\"" + regInfo.ExePath + "\" %1");
            relationKey.Close();
        }
        /// <summary>
        /// GetFileTypeRegInfo 得到指定文件类型关联信息
        /// </summary>        
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            if (!FileTypeRegistered(extendName))
            {
                return null;
            }
            FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName);
            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
            regInfo.Description = relationKey.GetValue("").ToString();
            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
            regInfo.IcoPath = iconKey.GetValue("").ToString();
            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command");
            string temp = commandKey.GetValue("").ToString();
            regInfo.ExePath = temp.Substring(0, temp.Length - 3);
            return regInfo;
        }
        /// <summary>
        /// UpdateFileTypeRegInfo 更新指定文件类型关联信息
        /// </summary>    
        public static bool UpdateFileTypeRegInfo(FileTypeRegInfo regInfo)
        {
            if (!FileTypeRegistered(regInfo.ExtendName))
            {
                return false;
            }

            string extendName = regInfo.ExtendName;
            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
            relationKey.SetValue("", regInfo.Description);
            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
            iconKey.SetValue("", regInfo.IcoPath);
            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command", true);
            commandKey.SetValue("", "\"" + regInfo.ExePath + "\" %1");
            relationKey.Close();
            return true;
        }
        /// <summary>
        /// FileTypeRegistered 指定文件类型是否已经注册
        /// </summary>        
        public static bool FileTypeRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
