namespace Journal.Security.Decryption
{
    public interface IDecrypter
    {
        string Decrypt(string encryptedContent,string password);
    }
}
