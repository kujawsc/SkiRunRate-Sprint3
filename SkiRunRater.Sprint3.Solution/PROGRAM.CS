using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiRunRater;

namespace SkiRunRater
{
    class Program
    {
        static void Main(string[] args)
        {
            // add test data to the data file
            InitializeDataFileXML_DS.AddTestData();

            // instantiate the controller
            Controller appContoller = new Controller();
        }
    }
}
