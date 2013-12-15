using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrofCryptographic.Utility
{
    class ConfigUtility
    {
        public static string GetDataEncrypto(string keyName)
        {
            var encrypoData = ConfigurationManager.AppSettings[keyName];

            if (encrypoData == null)
            {
                return "";
            }

            byte[] source = Convert.FromBase64String(encrypoData);
            var decodedData = Encoding.ASCII.GetString(source);
            return decodedData;

        }


    }
}
