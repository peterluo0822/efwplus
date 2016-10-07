using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WCFHosting
{
    public class TimeCDKEY
    {
        public static int InitRegedit(out string expireDate,out string identify)
        {
            expireDate = "";
            identify = "";
            /*检查注册表*/
            string SericalNumber = ReadSetting();    // 读取注册表， 检查是否注册 -1为未注册
            if (SericalNumber == "-1")
            {
                return 1;
            }
            /* 比较CPUid */
            string CpuId = TimeCDKEY.GetSoftEndDateAllCpuId(1, SericalNumber);   //从注册表读取CPUid
            string CpuIdThis = GetCpuId();           //获取本机CPUId         
            if (CpuId != CpuIdThis)
            {
                return 2;
            }
            /* 比较时间 */
            string NowDate = TimeCDKEY.GetNowDate();
            string EndDate = TimeCDKEY.GetSoftEndDateAllCpuId(0, SericalNumber);
            if (Convert.ToInt32(EndDate) - Convert.ToInt32(NowDate) < 0)
            {
                return 3;
            }
            DateTime dt = DateTime.ParseExact(EndDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            expireDate = dt.ToString("yyyy年MM月dd日");
            identify = TimeCDKEY.GetSoftEndDateAllCpuId(2, SericalNumber);

            return 0;
        }

        /*CPUid*/
        public static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }
        /*当前时间*/
        public static string GetNowDate()
        {
            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            return NowDate;
        }
        /* 生成序列号 */
        public static string CreatSerialNumber(string identify, string cpu, string expiredate)
        {
            if (string.IsNullOrEmpty(identify))
            {
                identify = DateTime.Now.Ticks.ToString();
            }
            if (string.IsNullOrEmpty(cpu))
            {
                cpu = GetCpuId();
            }
            if (string.IsNullOrEmpty(expiredate))
            {
                expiredate = DateTime.Now.AddYears(2).ToString("yyyyMMdd");
            }
            string SerialNumber = identify + "-" + cpu + "-" + expiredate;
            return SerialNumber;
        }
        /* 
         * i=2 得到中间件标识
         * i=1 得到 CUP 的id 
         * i=0 得到上次或者 开始时间 
         */
        public static string GetSoftEndDateAllCpuId(int i, string SerialNumber)
        {
            string[] data = SerialNumber.Split('-');
            if (data.Length == 3)
            {
                if (i == 2)
                {
                    return data[0];
                }
                if (i == 1)
                {
                    return data[1];
                }
                if (i == 0)
                {
                    return data[2];
                }

                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        /*写入注册表*/
        public static void WriteSetting(string Setting)
        {
            HostSettingConfig.SetValue("cdkey", Setting);
            HostSettingConfig.SaveConfig();
        }
        /*读取注册表*/
        public static string ReadSetting()
        {
            string key1 = HostSettingConfig.GetValue("cdkey");
            if (key1 != "")
            {
                return Encryption.DisEncryPW(key1, "kakake!@#123");
            }
            return "-1";
        }
    }

    public class Encryption
    {
        public static string EncryPW(string Pass, string Key)
        {
            return DesEncrypt(Pass, Key);
        }
        public static string DisEncryPW(string strPass, string Key)
        {
            return DesDecrypt(strPass, Key);
        }
        /////////////////////////////////////////////////////////////////////   

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString, string key)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch {
                return "-";
            }
        }
        //////////////////////////////////////////////////////
    }
}
