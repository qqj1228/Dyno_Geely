using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynoServer {
    class Program {
        static void Main(string[] args) {
            DynoServer server = new DynoServer(5555);
            server.ListenForClients();
        }
    }
}
