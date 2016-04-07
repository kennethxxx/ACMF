using System;
using System.Linq;

namespace WorkerRoleController.WRServiceManagement
{
    abstract class ServiceStatus
    {

        private int instancesNumber;

        public int InstanceNumber
        {
            get { return instancesNumber; }
            set { instancesNumber = value; }
        }

        public override string ToString()
        {
            return "Changing to " + instancesNumber + " instances.";
        }

    }

    class NextServiceStatus : ServiceStatus
    {
        private DateTime _nowTime;

        private bool _stop;
        private bool _delete;

        public NextServiceStatus()
        {
            _nowTime = Utils.Now;
        }

        public NextServiceStatus(DateTime now)
        {
            _nowTime = now;
        }

        public DateTime CurrentTime
        {
            get { return _nowTime; }
        }

        public bool Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        public bool Delete
        {
            get { return _delete; }
            set { _delete = value; }
        }

        public override string ToString()
        {
            if (_delete)
            {
                return "Delete";
            }
            else if (_stop)
            {
                return "Stop";
            }
            else return base.ToString();
        }
    }

    public enum RoleInstance
    {
        Initializing ,
        Ready,
        Busy,
        Stopping,
        Stopped,
        Unresponsive
    }

    public enum WRDeploymentStatus
    {
        Running ,
        Suspended,
        RunningTransitioning,
        SuspendedTransitioning,
        Starting,
        Suspending,
        Deploying,
        Deleting
    }
}
