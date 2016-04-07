using System;
using System.Linq;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class MonitoringWRPeriod
    {
        private int secondsPerLoop;
        private string scalingLogTableName;
        private string scalingLogTableEntitiesPartionKey;

        public int SecondsPerLoop
        {
            get
            {
                int num = this.secondsPerLoop;
                return num;
            }
            set
            {
                this.secondsPerLoop = value;
            }
        }

        public MonitoringWRPeriod()
        {
        }

        public string ScalingLogTableEntitiesPartionKey
        {
            get
            {
                string str = this.scalingLogTableEntitiesPartionKey;
                return str;
            }
            set
            {
                this.scalingLogTableEntitiesPartionKey = value;
            }
        }

        public string ScalingLogTableName
        {
            get
            {
                string str = this.scalingLogTableName;
                return str;
            }
            set
            {
                this.scalingLogTableName = value;
            }
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                MonitoringWRPeriod monitoringWRPeriod = (MonitoringWRPeriod)obj;
                flag2 = !this.secondsPerLoop.Equals(monitoringWRPeriod.secondsPerLoop);
                flag = (flag2 ? false : true);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.GetHashCode();
            return hashCode;
        }

        public bool Validate()
        {
            bool flag;
            flag = (this.secondsPerLoop > 0);
            bool flag1 = flag;
            return flag1;
        }


    }
}
