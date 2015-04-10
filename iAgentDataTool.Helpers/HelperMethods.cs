using iAgentDataTool.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using iAgentDataTool.Models.Remix;
using phc_DACommon;

namespace iAgentDataTool.Helpers
{
    public static class HelperMethods
    {
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        /// <summary>
        ///Loops over items as a IEnumerable and prints them to the Console
        /// </summary>
        public static void WriteToConsole(IEnumerable items)
        {
            foreach (object o in items)
            {
                Console.WriteLine(o.ToString());
            }
        }

        public static string DecryptStringBase64(this string value)
        {
            var decrypt = new phc_DACommon.Methods();
            try
            {
                decrypt.fGetDecryptedString(ref value);
                return value;
            }
            catch (Exception)
            {
                return value;
            }      
        }

        public static string EncryptStringBase64(this string value)
        {
            var encrypt = new phc_DACommon.Methods();
            encrypt.fGetEncryptedString(ref value);
            return value;
        }

        private static void DecryptCredentials(this PortalUser portalUser, ref string username, ref string password)
        {
            var decrypt = new phc_DACommon.Methods();
            username = portalUser.Username;
            password = portalUser.Password;
            decrypt.fGetDecryptedString(ref username);
            decrypt.fGetDecryptedString(ref password);
            portalUser.Password = password;
            portalUser.Username = username;
        }

        /// <summary>
        /// Encrypts a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>  
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encoded");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using a empty key");
            }
            var cspp = new CspParameters();
            cspp.KeyContainerName = key;
            var rsa = new RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;
            var bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);
            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// Decrypts a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToDecrypt">String that must be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            try
            {
                var cspp = new CspParameters();
                cspp.KeyContainerName = key;

                var rsa = new RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                var decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                var decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));

                var bytes = rsa.Decrypt(decryptByteArray, true);

                result = System.Text.UTF8Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                // no need for further processing
            }
            return result;
        }
 

    }
}
