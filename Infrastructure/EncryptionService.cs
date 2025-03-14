using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _key;
        private readonly string _iv;

        public EncryptionService()//string key, string iv)
        {
            // In production, these should be securely stored in configuration
            _key = "QPgXuYLktSTjJYW1M2ukbg==";// key;
            _iv = "6o47LNMi0U/jbWKvILbLYg==";// iv;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(_key);
                aesAlg.IV = Convert.FromBase64String(_iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText)) return cipherText;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Convert.FromBase64String(_key);
                    aesAlg.IV = Convert.FromBase64String(_iv);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch 
            {
                return cipherText;
            }
        }
    }

    //// Example entity with encrypted fields
    //public class Customer
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    [Encrypted] // Custom attribute to mark encrypted fields
    //    public string SocialSecurityNumber { get; set; }
    //    [Encrypted]
    //    public string CreditCardNumber { get; set; }
    //    public string Email { get; set; }
    //}



    // Custom type handler for Dapper
    public class EncryptedTypeHandler : SqlMapper.TypeHandler<string>
    {
        private readonly IEncryptionService _encryptionService;

        public EncryptedTypeHandler(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public override string Parse(object value)
        {
            if (value == null) return null;
            return _encryptionService.Decrypt(value.ToString());
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, string value)
        {
            parameter.Value = value == null ? (object)DBNull.Value : _encryptionService.Encrypt(value);
        }
    }

    //// Example repository implementation
    //public class CustomerRepository
    //{
    //    private readonly string _connectionString;
    //    private readonly EncryptionService _encryptionService;

    //    public CustomerRepository(string connectionString, EncryptionService encryptionService)
    //    {
    //        _connectionString = connectionString;
    //        _encryptionService = encryptionService;

    //        // Register the custom type handler
    //        SqlMapper.AddTypeHandler(new EncryptedTypeHandler(encryptionService));
    //    }

    //    public async Task<int> CreateCustomerAsync(Customer customer)
    //    {
    //        const string sql = @"
    //            INSERT INTO Customers (Name, SocialSecurityNumber, CreditCardNumber, Email)
    //            VALUES (@Name, @SocialSecurityNumber, @CreditCardNumber, @Email);
    //            SELECT CAST(SCOPE_IDENTITY() as int)";

    //        using (var connection = new SqlConnection(_connectionString))
    //        {
    //            return await connection.QuerySingleAsync<int>(sql, customer);
    //        }
    //    }

    //    public async Task<Customer> GetCustomerAsync(int id)
    //    {
    //        const string sql = "SELECT * FROM Customers WHERE Id = @Id";

    //        using (var connection = new SqlConnection(_connectionString))
    //        {
    //            return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
    //        }
    //    }
    //}
}