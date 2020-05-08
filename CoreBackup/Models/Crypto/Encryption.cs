﻿using Avalonia.Controls;
using System;
using System.IO;
using System.Security.Cryptography;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Diagnostics;
using System.Linq;
using CoreBackup.ViewModels;

namespace CoreBackup.Models.Crypto
{
    static class Encryption
    {
        public static bool isKeySet { get; set; }
        public static bool isIVSet { get; set; }
        public static bool isPathRemembered { get; set; }
        public static bool IsKeyLoaded = false;

        private static string KeyIVFilePath;

        private const int AES256KEYSIZE = 32;
        private const int AES256IVSIZE = 16;
        
        private static byte[] AES256Key;
        private static string AES256KeySTRING;

        private static byte[] AES256IV;
        private static string AES256IVString;

        public static void CreateAESKey()
        {
            AES256Key = CreateByteArray(AES256KEYSIZE);
            AES256KeySTRING = Convert.ToBase64String(AES256Key);
            Debug.WriteLine(AES256KeySTRING);
            isKeySet = true;
        }

        public static void CreateAESIV()
        {
            AES256IV = CreateByteArray(AES256IVSIZE);
            AES256IVString = Convert.ToBase64String(AES256IV);
            Debug.WriteLine(AES256IVString);
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
            string[] result = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                result = await dialog.ShowAsync(desktopLifetime.MainWindow);
            }
            try
            {
                if (result!=null && result.Length!=0)
                {
                    resultReturn = result[0];
                    fullPath = string.Join(" ", resultReturn);
                    Debug.WriteLine(fullPath);
                    byte[] buffer = File.ReadAllBytes(fullPath);
                    Debug.WriteLine(buffer.Length);
                    AES256Key = buffer.Take(32).ToArray();
                    AES256IV = buffer.Skip(32).Take(16).ToArray();
                    //Debug.WriteLine(Convert.ToBase64String(AES256Key));
                    //Debug.WriteLine(Convert.ToBase64String(AES256IV));
                    IsKeyLoaded = true;
                    EventLogViewModel.AddNewRegistry("AES Key and IV has been loaded from external file",
                        DateTime.Now, "Encryption", "HIGH");
                }

            } catch (IOException ex)
            {
                EventLogViewModel.AddNewRegistry("File that stores a key can not be opened",
                    DateTime.Now, "Encryption", "ERROR");
            }

        }

        public async static void SaveAES_KeyIV_ToFile()
        {
            string resultReturn = null;
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

            if (!String.IsNullOrEmpty(resultReturn))
            {
                byte[] concat = new byte[48];
                AES256Key.CopyTo(concat, 0);
                AES256IV.CopyTo(concat, 32);
                File.WriteAllBytes(resultReturn, concat);
                EventLogViewModel.AddNewRegistry("AES Key and IV has been saved to external file",
                    DateTime.Now, "Encryption", "HIGH");
            }
        }

        public static bool AESEncryptFile(string filePath, bool deletePlainFile)
        {
            if (true)
            {
                byte[] salt = CreateByteArray(2);
                // FileStream for Creating Encrypted File
                using (FileStream fs = new FileStream(filePath + ".enc", FileMode.Create))
                {
                    using (Aes aes = new AesManaged())
                    {
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
            if (true)
            {
                byte[] salt = CreateByteArray(2);
                int offset = 0;
                using (FileStream fsIn = new FileStream(filePath, FileMode.Open))
                {
                    fsIn.Read(salt, offset, salt.Length);
                    using (Aes aes = new AesManaged())
                    {
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
