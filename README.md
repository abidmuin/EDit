# EDit
EDit (Encrypt Decrypt it) is a file encryptor and decryptor.
This project implements a robust file encryption and decryption tool that combines symmetric and asymmetric cryptographic algorithms to protect data. The system is designed to securely encrypt files, generate digital signatures for integrity verification, and manage encryption keys through a manifest file. Key features include:

### 1. RSA Key Pair Generation:

The system generates RSA key pairs (2048-bit) for asymmetric encryption.
Public and private keys are output as XML strings for easy storage and retrieval.

### 2.File Encryption Using AES:

Files are encrypted using AES symmetric encryption with a 128-bit key size.
Randomly generated encryption keys and initialization vectors (IV) enhance security.
The EncryptFile method handles the encryption process using the AES algorithm.

### 3.Digital Signature Generation:

HMACSHA256 is used to create a digital signature of the encrypted file.
A random 64-byte signature key ensures the uniqueness and security of the signature.
The signature verifies the integrity of the encrypted data.

### 4.Key Management and Manifest Creation:

The AES encryption key, IV, and signature key are encrypted using the RSA public key.
Encrypted keys and signatures are stored in an XML manifest file.
The manifest (CreateManifest method) facilitates secure key distribution and backend parsing.

### 5.File Decryption:

Encrypted files can be decrypted using the corresponding AES key and IV.
The RSA private key is used to decrypt the AES key and signature key from the manifest.
The DecryptFile method handles the decryption process, restoring the original file.

### 6.Utility Functions:

Random byte generators (GenerateRandom) for keys and IVs.
RSA encryption and decryption for byte arrays (RSAEncryptBytes, RSADecryptBytes).
XML manipulation for storing and retrieving key information.

### 7.Use Case and Benefits:

- Secure Data Transfer: Ensures that sensitive files can be securely transmitted over unsecured channels.
- Access Control: Only recipients with the correct RSA private key can decrypt and access the files.
- Data Integrity: Digital signatures verify that the data has not been tampered with during transit.
- Scalability: The system can be integrated into larger applications requiring secure file handling.

### 8.Technical Highlights:

Utilizes standard cryptographic libraries in .NET (System.Security.Cryptography).
Implements best practices in key generation and management.
Separates encryption logic and key handling for modularity and maintainability.
