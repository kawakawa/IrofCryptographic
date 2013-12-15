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

        /// <summary>
        /// Twitterと接続
        /// </summary>
        public void Open()
        {
            var auth = Authorizer.DoApplicationOnly();
            auth.Authorize();

            _context = new TwitterContext(auth);
        }




        public List<LinqToTwitter.Status> GetAllTimeLineData(string targetUserId)
        {
            var list = new List<LinqToTwitter.Status>();
            
            var auth = Authorizer.DoApplicationOnly();

            auth.Authorize();

            var twitterCtx = new TwitterContext(auth);

            var subscribedList = twitterCtx.Status

                .Where(n => n.Type == StatusType.User)
                .Where(n => n.ScreenName == targetUserId)
                .Where(n=>n.Count==200)

                .ToList();

            return subscribedList; 
        }


    }
}
