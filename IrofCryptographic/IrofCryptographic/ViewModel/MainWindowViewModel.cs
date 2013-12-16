using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using IrofCryptographic.Command;
using IrofCryptographic.Model.Twitter;

namespace IrofCryptographic.ViewModel
{
     public class MainWindowViewModel:BaseViewModel
     {


         //秘密キー
         private LinqToTwitter.Status localKeyA;
         private LinqToTwitter.Status localKeyB;

         //秘密キー
         private string localKey;

         public string LocalKey
         {
             set
             {
                 this.localKey = value;
                 OnPropertyChanged("LocalKey");
             }
             get
             {
                 return this.localKey;
             }
         }

         public string NormalTxt
         {
             set;
             get;
         }

         private string encryptTxt;

         public  string EncryptTxt 
         {
             get
             {
                 return this.encryptTxt;
             }
             set
             {
                 this.encryptTxt = value;
                 OnPropertyChanged("EncryptTxt");
             }
    
         }




         public string EncryptTxt2
         {
             get; set;

         }

         private string normalTxt2;
         public string NormalTxt2
         {
             get
             {
                 return this.normalTxt2;
             }
             set
             {
                 this.normalTxt2 = value;
                 OnPropertyChanged("NormalTxt2");
             }
         }




         /// <summary>
         /// キー作成
         /// </summary>
         private ICommand _publicKeyCreateCommand;
         public ICommand publicKeyCreateCommand
        {
            get
            {
                if(_publicKeyCreateCommand == null)
                {
                    _publicKeyCreateCommand = new DelegateCommand(this.createKey);
                }
                return _publicKeyCreateCommand;
            }
        }

         /// <summary>
         /// 暗号化
         /// </summary>
         private ICommand _EncryptCommand;

         public ICommand EncryptCommand
         {
             get
             {
                 if (_EncryptCommand == null)
                 {
                     _EncryptCommand=new DelegateCommand(this.doEncrypt);
                 }
                 return _EncryptCommand;
             }
         }


         private ICommand _decryptCommand;

         public ICommand DecryptCommand
         {
             get
             {
                 if (_decryptCommand == null)
                 {
                     _decryptCommand=new DelegateCommand(this.doDecrypt);
                 }
                 return _decryptCommand;
             }
         }


         private string _urlA;

         public string UrlA
         {
             get
             {
                 return this._urlA;
             }
             set
             {
                 this._urlA = value;
                 OnPropertyChanged("UrlA");
             }
         }


         public Action<string> SetWebBrowserA
         {
             get;
             set;
         }
         public Action<string> SetWebBrowserB
         {
             get;
             set;
         }



         /// <summary>
         /// キー作成
         /// </summary>
         private void createKey()
         {
             var twitter = new TwitterData();
             twitter.Open();
             twitter.GetAllTimeLineData("irof");

             localKeyA = twitter.GetRandomStatus();
             localKeyB = twitter.GetRandomStatus(localKeyA.StatusID);


             //画面描画
             SetWebBrowserA("https://twitter.com/irof/statuses/"+localKeyA.StatusID);
             SetWebBrowserB("https://twitter.com/irof/statuses/" + localKeyB.StatusID);

             //公開キー作成
             this.createPublicKey();

             //あんごうか
             var hoge = this.Encript("WE　ラブ　いろふ");

             var hoge2 = Decript(hoge);

         }




         private void createPublicKey()
         {
             var hashA = this.getHash(this.localKeyA.Text);
             var hashB = this.getHash(this.localKeyB.Text);

             var byteKey = this.getXor(hashA, hashB);
             this.LocalKey = System.Convert.ToBase64String(byteKey);
         }


         private byte[] getHash(string txt)
         {
             var bytes = getBytes(txt);

             var md5 = MD5.Create();
             var hash = md5.ComputeHash(bytes);
             return hash;
         }


         private byte[] getBytes(string txt)
         {
             var bytes = ASCIIEncoding.ASCII.GetBytes(txt);
             return bytes;
         }





         private byte[] getXor(byte[] target, byte[] enc)
         {
             var result = new byte[target.Length > enc.Length ? target.Length : enc.Length];

             for (int i = 1; i < result.Length + 1; i++)
             {
                 if (i > target.Length)
                 {
                     result[result.Length - i] = enc[enc.Length - i];
                 }else if (i > enc.Length)
                 {
                     result[result.Length - i] = target[target.Length - i];
                 }
                 else
                 {
                     result[result.Length - i] =
                            (byte)(enc[enc.Length - i] ^ target[target.Length - i]);
                 }
             }
             return result;
         }




         public string Encript(string targetTxt)
         {
             var tmp = Convert.FromBase64String(this.localKey);

             using (SymmetricAlgorithm cspAlgorithm = new AesManaged())
             {
                 cspAlgorithm.Key = getByts(tmp,0, tmp.Length);
                 cspAlgorithm.IV = getByts(tmp, 0, tmp.Length);

                 using (ICryptoTransform encryptor = cspAlgorithm.CreateEncryptor())
                 {
                     byte[] source = Encoding.UTF8.GetBytes(targetTxt);
                     byte[] encrypted = encryptor.TransformFinalBlock(source, 0, source.Length);

                     return  Convert.ToBase64String(encrypted);
                 }

             }
         }



         private byte[] getByts(byte[] target,int begin, int end)
         {
             var query = target.Select((n, index) => new {index, n})
                 .Where(n => n.index >= begin)
                 .Where(n => n.index <= end)
                 .Select(n => n.n)
                 .ToArray();

             return query;

         }



         public string Decript(string encryptoTxt)
         {
             var tmp = Convert.FromBase64String(this.localKey);

             using (SymmetricAlgorithm cspAlgorithm = new AesManaged())
             {
                 cspAlgorithm.Key = getByts(tmp, 0, tmp.Length);
                 cspAlgorithm.IV = getByts(tmp, 0, tmp.Length);

                 using (ICryptoTransform encryptor = cspAlgorithm.CreateDecryptor())
                 {
                     byte[] source = Convert.FromBase64String(encryptoTxt);
                     byte[] decrypted = encryptor.TransformFinalBlock(source, 0, source.Length);

                     return Encoding.UTF8.GetString(decrypted);
                 }
             }

         }

         private void doEncrypt()
         {
             var encryptedText = this.Encript(this.NormalTxt);
             this.EncryptTxt = encryptedText;
         }

         private void doDecrypt()
         {
             var normal = Decript(this.EncryptTxt2);
             this.NormalTxt2 = normal;
         }

    }
}
