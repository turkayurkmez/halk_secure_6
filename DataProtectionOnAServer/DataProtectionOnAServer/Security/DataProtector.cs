using System.Security.Cryptography;
using System.Text;

namespace DataProtectionOnAServer.Security
{
    public class DataProtector
    {
        /*
         * Uygulamanızın koştuğu sunuculara güvenmeyin.
         *   -Obfuscator gibi araçlar kullanın ya da kritik datayı koruyun.
         */

        private string path;
        private byte[] enthropy = new byte[16]; //veriyi şifrelerken ya da çözümlerken kullanılacak byte array
        public DataProtector(string path)
        {
            this.path = path;
            enthropy = RandomNumberGenerator.GetBytes(16);
        }

        public int EncryptData(string value)
        {
            var encodedValue = Encoding.UTF8.GetBytes(value);
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            int length = encryptDataToFile(encodedValue, enthropy, DataProtectionScope.CurrentUser, fileStream);
            fileStream.Close();
            return length;
        }

        private int encryptDataToFile(byte[] encodedValue, byte[] enthropy, DataProtectionScope currentUser, FileStream fileStream)
        {
            byte[] encryptedData = ProtectedData.Protect(encodedValue, enthropy, currentUser);
            int outputLength = 0;

            if (fileStream.CanWrite && encryptedData != null)
            {
                fileStream.Write(encryptedData, 0, encryptedData.Length);
                outputLength = encryptedData.Length;
            }

            return outputLength;

        }

        public string DecryptData(int length)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            byte[] decrypt = decryptFromFile(fileStream, enthropy, DataProtectionScope.CurrentUser, length);
            fileStream.Close();
            return Encoding.UTF8.GetString(decrypt);

        }

        private byte[] decryptFromFile(FileStream fileStream, byte[] enthropy, DataProtectionScope currentUser, int length)
        {
            byte[] encyptedInput = new byte[length];
            byte[] decryptedOutput = new byte[length];

            if (fileStream.CanRead)
            {
                fileStream.Read(encyptedInput, 0, encyptedInput.Length);
                decryptedOutput = ProtectedData.Unprotect(encyptedInput, enthropy, currentUser);
            }

            return decryptedOutput;
        }
    }
}
