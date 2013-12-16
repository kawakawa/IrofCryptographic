using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

         //公開キー
         private String publicKey = "";


         /// <summary>
         /// 公開キー作成
         /// </summary>
         private ICommand _publicKeyCreateCommand;
         public ICommand publicKeyCreateCommand
        {
            get
            {
                if(_publicKeyCreateCommand == null)
                {
                    _publicKeyCreateCommand = new DelegateCommand(this.createLocalKey);
                }
                return _publicKeyCreateCommand;
            }
        }








         /// <summary>
         /// 秘密キー作成
         /// </summary>
         private void createLocalKey()
         {
             var twitter = new TwitterData();
             twitter.Open();
             twitter.GetAllTimeLineData("irof");

             localKeyA = twitter.GetRandomStatus();
             localKeyB = twitter.GetRandomStatus();

             //画面描画


             //公開キー作成
             this.createPublicKey();

         }

         private void createPublicKey()
         {
             var hashA = this.getHash(this.localKeyA.Text);
             var hashB = this.getHash(this.localKeyB.Text);

             var byteKey = this.getXor(hashA, hashB);
             publicKey = Encoding.ASCII.GetString(byteKey);
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




    }
}
