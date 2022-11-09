using PasswordGenerator;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;

passwordBuilder methods = new passwordBuilder();

string password = "TestingPassword123!";
var passwordBytes = methods.PasswordInput(password);

var saltBytes = methods.SaltGetter();
var saltString = Encoding.UTF8.GetString(saltBytes);
//just to see what the salt has as its value in string. will not be used.
var aesInstance = methods.AesGetter();
//aes initialization. the aesinstance will be shared across both the encrpytor and decryptor, with the same key and iv being used for both.

var encryptedArray = methods.aesEncrypt(aesInstance, passwordBytes, saltBytes);
var encryptedString = Encoding.UTF8.GetString(encryptedArray);
//string preview of the encrypted array. will not be used.

var decryptedString = methods.aesDecrypt(encryptedArray, aesInstance);
//decrypted string is what will be displayed. should be equal to saltString + password.
var decryptedArray = Encoding.UTF8.GetBytes(decryptedString);
//array version preview of decryptedstring. for testing purposes only.

Console.WriteLine(decryptedString);


