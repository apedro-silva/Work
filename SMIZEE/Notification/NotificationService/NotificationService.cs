using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace SF.Expand.Notification
{
    public static class NotificationService
    {
        /// <summary>
        /// Send notification.
        /// </summary>
        public static void Send(string notificationParms, string notificationServiceTypeName, string destinationTO, string subject, string messageBody)
        {
            string previousNotificationServiceTypeName = string.Empty;
            INotification notificationAssembly = null;
            int requestTimeout=10;
            string[] baseParams = notificationParms.Split(';');
            string destinationCC = string.Empty;

            notificationAssembly = NotificationFactory.LoadAssembly(notificationServiceTypeName);
            if (notificationAssembly == null)
                throw new Exception(notificationServiceTypeName + " assembly does not exist in the site");

            notificationAssembly.Send(baseParams, requestTimeout, subject, messageBody, destinationTO, destinationCC, null);

        }

    }
}
