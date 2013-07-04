using System;
using System.Collections.Generic;
using System.Text;

namespace SF.Expand.Notification
{
    public interface INotification
    {
        void Send(string[] baseParams, int requestTimeout, string subject, string messageBody, string destinationTO, string destinationCC, string attachementPatch);
    }
}
