using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RCSSerivce
{
    public class EncryptionService
    {
        public string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        public string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }

    //Aadhar Card Validations
    public static class Verhoeff
    {
        private static int[,] d = new int[10, 10]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
            { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
            { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
            { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
            { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
            { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
            { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
            { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
            { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
            { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
        };
        private static int[,] p = new int[8, 10]
        {
             { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
             { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
             { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
             { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
             { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
             { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
             { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
             { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
        };

        private static int[] inv = new int[10]{ 0, 4, 3, 2, 1, 5, 6, 7, 8, 9};

        public static bool validateVerhoeff(string num)
        {
            int index1 = 0;
            int[] reversedIntArray = Verhoeff.StringToReversedIntArray(num);
            for (int index2 = 0; index2 < reversedIntArray.Length; ++index2)
                index1 = Verhoeff.d[index1, Verhoeff.p[index2 % 8, reversedIntArray[index2]]];
            return index1 == 0;
        }

        public static string generateVerhoeff(string num)
        {
            int index1 = 0;
            int[] reversedIntArray = Verhoeff.StringToReversedIntArray(num);
            for (int index2 = 0; index2 < reversedIntArray.Length; ++index2)
                index1 = Verhoeff.d[index1, Verhoeff.p[(index2 + 1) % 8, reversedIntArray[index2]]];
            return Verhoeff.inv[index1].ToString();
        }

        private static int[] StringToReversedIntArray(string num)
        {
            int[] numArray = new int[num.Length];
            for (int startIndex = 0; startIndex < num.Length; ++startIndex)
                numArray[startIndex] = int.Parse(num.Substring(startIndex, 1));
            Array.Reverse((Array)numArray);
            return numArray;
        }
    }
}
