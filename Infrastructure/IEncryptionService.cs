namespace Infrastructure
{
    public interface IEncryptionService
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}