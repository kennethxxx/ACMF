using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PrivateWorker
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            WorkerRole worker1 = new WorkerRole();
            worker1.MultiplayerArchitecture();
        }

    }
}
