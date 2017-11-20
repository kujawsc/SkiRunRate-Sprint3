using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkiRunRater
{
    /// <summary>
    /// method to write all ski run information to the date file
    /// </summary>
    public class SkiRunRepositoryXML : IDisposable
    {
        private List<SkiRun> _skiRuns;

        public SkiRunRepositoryXML()
        {
            _skiRuns = ReadSkiRunsData(DataSettings.dataFilePath);
        }

        /// <summary>
        /// method to read all ski run information from the data file and return it as a list of SkiRun objects
        /// </summary>
        /// <param name="dataFilePath">path the data file</param>
        /// <returns>list of SkiRun objects</returns>
        public List<SkiRun> ReadSkiRunsData(string dataFilePath)
        {
            SkiRuns skiRunsFromFile = new SkiRuns();

            // initialize a FileStream object for reading
            StreamReader sReader = new StreamReader(DataSettings.dataFilePath);

            // initialize an XML seriailizer object
            XmlSerializer deserializer = new XmlSerializer(typeof(SkiRuns));

            using (sReader)
            {
                object xmlObject = deserializer.Deserialize(sReader);
                skiRunsFromFile = (SkiRuns)xmlObject;
            }

            return skiRunsFromFile.skiRuns;
        }

        /// <summary>
        /// method to write all of the list of ski runs to the text file
        /// </summary>
        public void WriteSkiRunsData()
        {
            // initialize a FileStream object for reading
            StreamWriter sWriter = new StreamWriter(DataSettings.dataFilePath, false);

            XmlSerializer serializer = new XmlSerializer(typeof(List<SkiRun>), new XmlRootAttribute("SkiRuns"));

            using (sWriter)
            {
                serializer.Serialize(sWriter, _skiRuns);
            }
        }

        /// <summary>
        /// method to add a new ski run
        /// </summary>
        /// <param name="skiRun"></param>
        public void InsertSkiRun(SkiRun skiRun)
        {
            _skiRuns.Add(skiRun);

            WriteSkiRunsData();
        }

        /// <summary>
        /// method to delete a ski run by ski run ID
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteSkiRun(int ID)
        {
            _skiRuns.RemoveAt(GetSkiRunIndex(ID));

            WriteSkiRunsData();
        }

        /// <summary>
        /// method to update an existing ski run
        /// </summary>
        /// <param name="skiRun">ski run object</param>
        public void UpdateSkiRun(SkiRun skiRun)
        {
            DeleteSkiRun(skiRun.ID);
            InsertSkiRun(skiRun);

            WriteSkiRunsData();
        }

        /// <summary>
        /// method to return a ski run object given the ID
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>ski run object</returns>
        public SkiRun GetSkiRunByID(int ID)
        {
            SkiRun skiRun = null;

            skiRun = _skiRuns[GetSkiRunIndex(ID)];

            return skiRun;
        }

        /// <summary>
        /// method to return a list of ski run objects
        /// </summary>
        /// <returns>list of ski run objects</returns>
        public List<SkiRun> GetSkiAllRuns()
        {
            return _skiRuns;
        }

        /// <summary>
        /// method to return the index of a given ski run
        /// </summary>
        /// <param name="skiRun"></param>
        /// <returns>int ID</returns>
        private int GetSkiRunIndex(int ID)
        {
            int skiRunIndex = 0;

            for (int index = 0; index < _skiRuns.Count(); index++)
            {
                if (_skiRuns[index].ID == ID)
                {
                    skiRunIndex = index;
                }
            }

            return skiRunIndex;
        }

        /// <summary>
        /// method to query the data by the vertical of each ski run in feet
        /// </summary>
        /// <param name="minimumVertical">int minimum vertical</param>
        /// <param name="maximumVertical">int maximum vertical</param>
        /// <returns></returns>
        public List<SkiRun> QueryByVertical(int minimumVertical, int maximumVertical)
        {
            List<SkiRun> matchingSkiRuns = new List<SkiRun>();

            foreach (var skiRun in _skiRuns)
            {
                if ((skiRun.Vertical >= minimumVertical) && (skiRun.Vertical <= maximumVertical))
                {
                    matchingSkiRuns.Add(skiRun);
                }
            }

            return matchingSkiRuns;
        }

        /// <summary>
        /// method to handle the IDisposable interface contract
        /// </summary>
        public void Dispose()
        {
            _skiRuns = null;
        }
    }
}
