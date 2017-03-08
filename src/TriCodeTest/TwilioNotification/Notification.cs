using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TriCodeTest.TwilioNotification
{
    /// <summary>
    /// Notification. Sends notification 
    /// </summary>
    public class Notification
    {
        //private readonly TwilioRestClient twilio;
        /// <summary>
        /// Send notifications
        /// </summary>
        /// <param name="phoneNumber">phoneNumber</param>
        /// <param name="body">body</param>
        /// <returns></returns>

        public static bool SendNotification(string phoneNumber, string body)
        {
            TwilioClient.Init(Credentials.AcctSID, Credentials.AcctToken);
            var to = new PhoneNumber(phoneNumber);
            var message = MessageResource.Create(
            to,
            from: new PhoneNumber(Credentials.TwilioNumber),
            body: body);
            if (message.Status.Equals(404))
            {
                return false;
            } else
            {
                return true;
            }
            
        }
    }
}
