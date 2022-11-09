using System.Security.Cryptography;
using System.Text;


namespace PasswordGenerator
{
    public class passwordBuilder
    {
        private readonly string _key = "35ca02e0-5cd3-4aa9-95f6-e6bdda9a17db";

        private RandomOrgClient initialize()
        {
            RandomOrgClient RClient = RandomOrgClient.GetRandomOrgClient(_key, 2000, 10000, true);
            return RClient;
            //uses Random.Org to generate a random string. Method used in SaltGetter()
        }
    
        public byte[] PasswordInput(string password)
        {
            var PasswordBase = new PasswordClass(password);
            string PasswordString = PasswordBase.Base;
            byte[] PasswordArray = Encoding.UTF8.GetBytes(PasswordString);

            return PasswordArray;
        }
        public byte[] SaltGetter()
        {
            RandomOrgClient client = initialize();
            string[] response;
            byte[] responseArray;
            try
            {

                 response = client.GenerateBlobs(32, 64, "base64");
                responseArray = Encoding.UTF8.GetBytes(response[0]);
                
                return responseArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                //create a backup random generator.
            }
        }
        public byte[] KeyGetter()
        {
            using(AesCng key = new AesCng())
            {
                key.GenerateKey();
                return key.Key;
            }
        }
        public byte[] IVGetter()
        {
            using(AesCng IV = new AesCng())
            {
                IV.GenerateIV();
                return IV.IV;
            }
        }
        public Aes AesGetter()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.ISO10126 | PaddingMode.Zeros;
            //it's necessary to use zeros, as for some reason it doesn't accept anything else as padding mode.
            aes.Key = KeyGetter();
            aes.IV = IVGetter();
            aes.BlockSize = 128;

            return aes;
        }
        
        public byte[] aesEncrypt(Aes aes, byte[]baseArray, byte[]saltArray)
        {
            byte[] ToEncrypt = new byte[saltArray.Length + baseArray.Length];
            //combination of password base with the salt

            Buffer.BlockCopy(saltArray, 0, ToEncrypt, 0, saltArray.Length);//size
            Buffer.BlockCopy(baseArray, 0, ToEncrypt, saltArray.Length, baseArray.Length);

            using (aes)
            {   //aes is the international standard. this var aes is merely a class that allows implementation of the functionality
                using (var enc = aes.CreateEncryptor(aes.Key,aes.IV))
                {//just an encryptor
                    using (var ms = new MemoryStream())
                    {//memory stream is just a stream which reads from the temporary memory, not the harddrive storage. assumedly short term. seems to be a tool for other processes.
                        using (var cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {//cryptostream reads from the memory stream and encrypts/decrypts data coming from it
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                cs.Write(ToEncrypt);
                                var encrypted = ms.ToArray();
                                return encrypted;
                            }
                        }
                    }
                }
            }
        }
        public string aesDecrypt(byte[] encrypted, Aes aes)
        {
            using (aes)
            {
                using(var dec = aes.CreateDecryptor(aes.Key,aes.IV))
                {
                    using (var ms = new MemoryStream(encrypted))
                    {
                        using (var cs = new CryptoStream(ms,dec, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                var decryptedText = sr.ReadToEnd();
                                return decryptedText;
                            } 
                        }
                    }
                }
            }
        }
    }
}
