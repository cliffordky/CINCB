namespace Core
{
    public interface IEncryptionService
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}