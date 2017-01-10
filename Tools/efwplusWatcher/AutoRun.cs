using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;//添加命名空间

namespace efwplusWatcher
{
    public class AutoRun
    {

        /// <summary>
        /// 设置开机自启动
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool SetAutoRun(string keyName, string filePath)
        {
            try
            {
                RegistryKey runKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                runKey.SetValue(keyName, filePath);
                runKey.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 取消开机自启动
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool DeleteAutoRun(string keyName)
        {
            try
            {
                RegistryKey runKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                runKey.DeleteValue(keyName, false);
                runKey.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
