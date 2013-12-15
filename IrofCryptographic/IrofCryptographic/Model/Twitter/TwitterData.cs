using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace IrofCryptographic.Model.Twitter
{




    public class TwitterData
    {

        private TwitterContext _context;

        private List<LinqToTwitter.Status> _targetUserTweetList =new List<LinqToTwitter.Status>();


        /// <summary>
        /// Twitterと接続
        /// </summary>
        public void Open()
        {
            var auth = Authorizer.DoApplicationOnly();
            auth.Authorize();

            _context = new TwitterContext(auth);
        }




        public void GetAllTimeLineData(string targetUserId)
        {
            
            var auth = Authorizer.DoApplicationOnly();

            auth.Authorize();

            var twitterCtx = new TwitterContext(auth);

            var list = twitterCtx.Status

                .Where(n => n.Type == StatusType.User)
                .Where(n => n.ScreenName == targetUserId)
                .Where(n=>n.Count==200)

                .ToList();

            _targetUserTweetList = list;
        }



        public Status GetRandomStatus()
        {
            if (this._targetUserTweetList.Count == 0)
            {
                return null;
            }


            var rand = new Random();
            int toSkip = rand.Next(0, this._targetUserTweetList.Count-1);

            var status=this._targetUserTweetList.Skip(toSkip).Take(1).FirstOrDefault();
            return status;
        }

    }
}
