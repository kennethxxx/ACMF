using System;
using System.Linq;

namespace WorkerRoleController.WRServiceManagement
{
    public class ScallingOperation
    {
        private const int NullInstances=-1;
        private readonly ServiceOperation operation;
        private readonly int count;
        private readonly String aux;

        public ScallingOperation(ServiceOperation operation)
            : this(operation, NullInstances)
        {

        }

        public ScallingOperation(ServiceOperation operation, int instanceCount)
        {
            this.operation=operation;
            this.count=instanceCount;
        }

        public ScallingOperation(ServiceOperation operation, String aux)
        {
            this.operation = operation;
            this.aux = aux;
            count = -1;
        }

        public String DeploymentInfo
        {
            get { return aux; }
        }

        public int InstanceCount
        {
            get { return count; }
        }

        public ServiceOperation Operation 
        {
            get { return operation; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ScallingOperation operation = (ScallingOperation)obj;
            return this.count == operation.InstanceCount && this.operation == operation.Operation;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode()+count;
        }

    }
}
