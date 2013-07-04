using System;
using System.Collections.Generic;
using System.Text;

namespace SF.Expand.Notification
{
    public class NotificationFactory
    {
        public static INotification LoadAssembly(string typeName)
        {
            if (typeName == null || typeName.Length == 0)
            {
                return null;
            }

            try
            {
                Type _type = Type.GetType(typeName, true);
                if (!typeof(INotification).IsAssignableFrom(_type))
                {
                    return null;
                }
                return (INotification)Activator.CreateInstance(_type, true);
            }
            catch
            {
                return null;
            }
        }

    }
}
