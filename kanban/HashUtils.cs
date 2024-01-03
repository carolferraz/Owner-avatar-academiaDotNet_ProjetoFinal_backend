using System.Security.Cryptography;

namespace kanbanBoard
{
    public static class HashUtils
    {


        private static byte[] GeraSalt()
        {
            byte[] salt = new byte[16]; // Tamanho do salt (pode ser ajustado conforme necessário)
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static byte[] SenhaHashComSalt(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // Tamanho do hash (neste exemplo, usamos 32 bytes)
            }
        }
        public static string GeraSenhaHash(string password)
        {
            byte[] salt = GeraSalt();
            byte[] hash = SenhaHashComSalt(password, salt);

            // Combine salt + hash e converta para uma string para armazenamento seguro
            string combinedSaltAndHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            return combinedSaltAndHash;
        }

        public static bool ValidaSenha(string password, string hashedPassword)
        {
            try
            {
                // Recupera o salt e hash a partir da string combinada
                string[] parts = hashedPassword.Split(':');
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] hash = Convert.FromBase64String(parts[1]);

                // Gera o hash da senha fornecida usando o salt recuperado
                byte[] testHash = SenhaHashComSalt(password, salt);

                // Compara os hashes
                return hash.SequenceEqual(testHash);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
