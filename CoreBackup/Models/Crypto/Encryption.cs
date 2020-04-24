using Avalonia.Controls;
using System;
using System.IO;
using System.Security.Cryptography;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Diagnostics;

namespace CoreBackup.Models.Crypto
{
    static class Encryption
    {
        public static bool isKeySet { get; set; }
        public static bool isIVSet { get; set; }
        public static bool isPathRemembered { get; set; }

        private static string KeyIVFilePath;

        private const int AES256KEYSIZE = 32;
        private const int AES256BLOCKSIZE = 32;
        
        private static byte[] AES256Key;
        private static string AES256KeySTRING;

        private static byte[] AES256IV;
        private static string AES256IVString;

        public static void CreateAESKey()
        {
            AES256Key = CreateByteArray(AES256KEYSIZE);
            AES256KeySTRING = Convert.ToBase64String(AES256Key);
            isKeySet = true;
        }

        public static void CreateAESIV()
        {
            AES256IV = CreateByteArray(AES256BLOCKSIZE);
            AES256IVString = Convert.ToBase64String(AES256IV);
            isIVSet = true;
        }

        public static byte[] CreateByteArray(int length)
        {
            byte[] result = new byte[length];
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(result);
                return result;
            }
        }

        public async static void LoadAES_KeyIV_FromFile()
        {
            string resultReturn = null;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result[0];
                fullPath = string.Join(" ", resultReturn);
            }
            try
            {
                // This code is somehow corrupted
                /*
                Stream file = File.OpenRead(resultReturn);
                file.Read(AES256Key, 0, 32);
                file.Read(AES256IV, 32, 32);
                file.Close();
                */

            } catch (IOException ex)
            {
                Debug.WriteLine("Cannot open key file");
            }

        }

        public async static void SaveAES_KeyIV_ToFile()
        {
            string resultReturn = null;
            //string fullPath = null;
            //saveFileDialog1.Filters = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save AES key to file";
                saveFileDialog1.InitialFileName = "core.key";
                saveFileDialog1.DefaultExtension = "key";
                string result = await saveFileDialog1.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result;
                Debug.WriteLine(resultReturn);
            }
            // TODO try/catch
            byte[] concat = new byte[64];
            AES256Key.CopyTo(concat, 0);
            AES256IV.CopyTo(concat, 32);
            File.WriteAllBytes(resultReturn, concat);
        }

        public static bool AESEncryptFile(string filePath, bool deletePlainFile)
        {
            bool KeyCondition = AES256Key != null && AES256Key.Length > 0;
            bool IVCondition = AES256IV != null && AES256IV.Length > 0;
            if (KeyCondition && IVCondition)
            {
                byte[] salt = CreateByteArray(16);
                // FileStream for Creating Encrypted File
                using (FileStream fs = new FileStream(filePath + ".enc", FileMode.Create))
                {
                    using (Aes aes = new AesManaged())
                    {
                        aes.KeySize = AES256KEYSIZE;
                        aes.Key = AES256Key;
                        aes.IV = AES256IV;
                        aes.Padding = PaddingMode.ISO10126;
                        aes.Mode = CipherMode.CBC;
                        int offset = 0;

                        fs.Write(salt, offset, salt.Length);
                        // FileStream for Encrypting 
                        using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            // FileStream for Opening Plain File
                            using (FileStream fsIn = new FileStream(filePath, FileMode.Open))
                            {
                                byte[] buffer = new byte[1];
                                int read;
                                try
                                {
                                    while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        cs.Write(buffer, 0, read);
                                    }

                                    if (deletePlainFile)
                                    {
                                        File.Delete(filePath);
                                    }

                                    cs.Close();
                                    fs.Close();
                                    fsIn.Close();

                                    return true;
                                }
                                catch (Exception e)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }


        public static bool AESDecryptFile(string filePath, bool keepEncryptedFile)
        {
            bool KeyCondition = AES256Key != null && AES256Key.Length > 0;
            bool IVCondition = AES256IV != null && AES256IV.Length > 0;
            if (KeyCondition && IVCondition)
            {
                byte[] salt = CreateByteArray(16);
                int offset = 0;
                using (FileStream fsIn = new FileStream(filePath, FileMode.Open))
                {
                    fsIn.Read(salt, offset, salt.Length);
                    using (Aes aes = new AesManaged())
                    {
                        aes.KeySize = AES256KEYSIZE;
                        aes.Key = AES256Key;
                        aes.IV = AES256IV;
                        aes.Padding = PaddingMode.ISO10126;
                        aes.Mode = CipherMode.CBC;

                        using (CryptoStream cs = new CryptoStream(fsIn, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOut = new FileStream(filePath.Remove(filePath.Length - 4),
                                FileMode.Create))
                            {
                                byte[] buffer = new byte[1];
                                int read;

                                try
                                {
                                    while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                                    { 
                                        fsOut.Write(buffer, 0, buffer.Length);
                                    }

                                    if (!keepEncryptedFile)
                                    {
                                        File.Delete(filePath);
                                    }

                                    cs.FlushFinalBlock();
                                    fsOut.Close();
                                    fsIn.Close();
                                    cs.Close();

                                    return true;

                                }
                                catch (Exception e)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }

}
