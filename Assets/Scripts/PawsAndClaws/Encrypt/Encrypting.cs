using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace PawsAndClaws.Encrypting
{
    public class RSAEncryptor
    {
        private RSACryptoServiceProvider _publicCrypto;
        private RSACryptoServiceProvider _privateCrypto;

        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public void GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);

            _publicCrypto = new RSACryptoServiceProvider();
            _privateCrypto = new RSACryptoServiceProvider();

            _publicCrypto.ImportParameters(_publicKey);
            _privateCrypto.ImportParameters(_privateKey);
        }

        public void setPublicKeyBytes(byte[] data)
        {
            _publicKey = Utils.BinaryUtils.ByteArrayToObject<RSAParameters>(data);

            _publicCrypto = new RSACryptoServiceProvider();
            _publicCrypto.ImportParameters(_publicKey);
        }

        public byte[] getPublicKeyBytes()
        {
            return Utils.BinaryUtils.ObjectToByteArray(_publicKey);
        }

        public byte[] encrypt(byte[] data)
        {
            return _publicCrypto.Encrypt(data, false);
        }

        public byte[] decrypt(byte[] data)
        {
            return _privateCrypto.Decrypt(data, false);
        }
    }
}