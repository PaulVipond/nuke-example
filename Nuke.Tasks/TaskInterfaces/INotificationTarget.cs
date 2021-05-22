using Nuke.Common;
using System;

namespace Nuke.Tasks.TaskInterfaces
{
    public interface INotificationTarget
    {
        Target Notification => null;
    }
}
