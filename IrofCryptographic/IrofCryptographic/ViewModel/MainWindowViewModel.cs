using System;
using System.Collections.Generic;
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


             //公開キー作成
             this.createPublicKey();

             //あんごうか
             var hoge = this.Encript("AAA");

             var hoge2 = Decript(hoge);

         }




         private void createPublicKey()
         {
             var hashA = this.getHash(this.localKeyA.Text);
             var hashB = this.getHash(this.localKeyB.Text);

             var byteKey = this.getXor(hashA, hashB);
             this.localKey = System.Convert.ToBase64String(byteKey);
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

             using (SymmetricAlgorithm rc2Csp = new RC2CryptoServiceProvider())
             {
                 rc2Csp.Key = getByts(tmp,3, tmp.Length);
                 rc2Csp.IV = getByts(tmp,0, 7);

                 using (ICryptoTransform encryptor = rc2Csp.CreateEncryptor())
                 {
                     byte[] source = Encoding.UTF8.GetBytes(targetTxt);
                     byte[] encrypted = encryptor.TransformFinalBlock(source, 0, source.Length);

                     return  Convert.ToBase64String(encrypted);
                 }

             }
             return "";
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

             using (SymmetricAlgorithm rc2Csp = new RC2CryptoServiceProvider())
             {
                 rc2Csp.Key = getByts(tmp, 3, tmp.Length);
                 rc2Csp.IV = getByts(tmp, 0, 7);

                 using (ICryptoTransform encryptor = rc2Csp.CreateDecryptor())
                 {
                     byte[] source = Convert.FromBase64String(encryptoTxt);
                     byte[] decrypted = encryptor.TransformFinalBlock(source, 0, source.Length);

                     return Encoding.UTF8.GetString(decrypted);
                 }
             }
             return "";

         }




    }
}
