using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;

namespace SkiRunRater
{
    /// <summary>
    /// method to write all ski run information to the date file
    /// </summary>
    public class SkiRunRepositoryXML_DS : IDisposable
    {
        DataSet _skiRuns_ds = new DataSet();
        DataTable _skiRuns_dt = new DataTable();

        public SkiRunRepositoryXML_DS()
        {
            ReadSkiRunsData();
            _skiRuns_ds.DataSetName = "SkiRuns";
            _skiRuns_dt = _skiRuns_ds.Tables[0];
        }

        /// <summary>
        /// method to read all ski run information from the XML data file and return it as a list of SkiRun objects
        /// </summary>
        /// <param name="dataFilePath">path the data file</param>
        /// <returns>list of SkiRun objects</returns>
        public void ReadSkiRunsData()
        {
            // create a dataset to hold the data from the reader
            DataSet ds = new DataSet();

            // create an XmlReader object
            XmlReader xmlReader = XmlReader.Create(DataSettings.dataFilePath);

            // read data file into DataSet
            _skiRuns_ds.ReadXml(xmlReader);

            // close XmlReader
            xmlReader.Close();
        }

        /// <summary>
        /// write the dataset to the xml file
        /// </summary>
        public void WriteSkiRunsData()
        {
            // create a XmlWriterSettings object to set the writing method
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            // create XmlWrtier object
            XmlWriter xmlWriter = XmlWriter.Create(DataSettings.dataFilePath, settings);

            // write DataSet to data file
            _skiRuns_ds.WriteXml(xmlWriter);

            // close DataWrtier
            xmlWriter.Close();
        }

        /// <summary>
        /// method to return a list of ski run objects
        /// </summary>
        /// <returns>list of ski run objects</returns>
        public List<SkiRun> SelectAllRuns()
        {
            List<SkiRun> skiRuns = new List<SkiRun>();

            foreach (DataRow skiRun in _skiRuns_dt.Rows)
            {
                skiRuns.Add(new SkiRun
                {
                    ID = int.Parse(skiRun["ID"].ToString()),
                    Name = skiRun["Name"].ToString(),
                    Vertical = int.Parse(skiRun["Vertical"].ToString()),
                });
            }

            return skiRuns;
        }

        /// <summary>
        /// method to return a ski run object given the ID
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>ski run object</returns>
        public SkiRun SelectByID(int ID)
        {
            SkiRun skiRun = new SkiRun();
            DataRow[] skiRuns;

            // get an array of ski runs with the matching ID
            skiRuns = _skiRuns_dt.Select("ID = " + ID.ToString());

            // no ski runs with the matching ID
            if (skiRuns.Count() < 1)
            {
                throw new Exception("Ski run now found.");
            }
            // multiple ski runs with the matching ID
            else if (skiRuns.Count() > 1)
            {
                throw new Exception("More than one ski run with the id: " + ID);
            }
            // a unique ski run with the matching ID
            else
            {
                skiRun.ID = int.Parse(skiRuns[0]["ID"].ToString());
                skiRun.Name = skiRuns[0]["Name"].ToString();
                skiRun.Vertical = int.Parse(skiRuns[0]["Vertical"].ToString());
            }

            return skiRun;
        }

        /// <summary>
        /// method to add a new ski run
        /// </summary>
        /// <param name="skiRun"></param>
        public void InsertSkiRun(SkiRun skiRun)
        {
            fillRow(_skiRuns_dt, skiRun.ID, skiRun.Name, skiRun.Vertical);
        }

        /// <summary>
        /// method to delete a ski run by ski run ID
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteSkiRun(int ID)
        {
            DataRow[] skiRuns;

            // get an array of ski runs with the matching ID
            skiRuns = _skiRuns_dt.Select("ID = " + ID.ToString());

            // no ski runs with the matching ID
            if (skiRuns.Count() < 1)
            {
                throw new Exception("Ski run now found.");
            }
            // multiple ski runs with the matching ID
            else if (skiRuns.Count() > 1)
            {
                throw new Exception("More than one ski run with the id: " + ID);
            }
            // a unique ski run with the matching ID
            else
            {
                _skiRuns_dt.Rows.Remove(skiRuns[0]);
            }
        }

        /// <summary>
        /// method to update an existing ski run
        /// </summary>
        /// <param name="skiRun">ski run object</param>
        public void UpdateSkiRun(SkiRun skiRun)
        {
            DataRow[] skiRuns;
            int ID = skiRun.ID;

            // get an array of ski runs with the matching ID
            skiRuns = _skiRuns_dt.Select("ID = " + ID.ToString());

            // no ski runs with the matching ID
            if (skiRuns.Count() < 1)
            {
                throw new Exception("Ski run now found.");
            }
            // multiple ski runs with the matching ID
            else if (skiRuns.Count() > 1)
            {
                throw new Exception("More than one ski run with the id: " + ID);
            }
            // a unique ski run with the matching ID
            else
            {
                skiRuns[0]["ID"] = skiRun.ID;
                skiRuns[0]["Name"] = skiRun.Name;
                skiRuns[0]["Vertical"] = skiRun.Vertical;
            }
        }

        /// <summary>
        /// method to query the data by the vertical of each ski run in feet
        /// </summary>
        /// <param name="minimumVertical">int minimum vertical</param>
        /// <param name="maximumVertical">int maximum vertical</param>
        /// <returns></returns>
        public List<SkiRun> QueryByVertical(int minimumVertical, int maximumVertical)
        {
            // create a list to hold the matching ski runs
            List<SkiRun> matchingSkiRuns = new List<SkiRun>();
            
            // create an array of DataRows to hold the matching ski runs
            DataRow[] skiRuns;

            // generate the a query string based on the values of the arguements sent
            string queryString = String.Format("Vertical > {0} AND Vertical < {1}", minimumVertical, maximumVertical);

            // use the Select method to fill the array of ski runs
            skiRuns = _skiRuns_dt.Select(queryString);

            // for each row in the array, create a new SkiRun object and add it to the list
            foreach (DataRow skiRun in skiRuns)
            {
                matchingSkiRuns.Add(new SkiRun() 
                {
                    ID = int.Parse(skiRun["ID"].ToString()),
                    Name = skiRun["Name"].ToString(),
                    Vertical = int.Parse(skiRun["Vertical"].ToString()),
                });
            }

            return matchingSkiRuns;
        }

        /// <summary>
        /// add a row of data to the DataTable
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="ID">ski run ID</param>
        /// <param name="name">ski run Name</param>
        /// <param name="vertical">ski run vertical</param>
        private static void fillRow(DataTable dt, int ID, string name, int vertical)
        {
            DataRow dr;
            dr = dt.NewRow();

            dr["ID"] = ID;
            dr["Name"] = name;
            dr["Vertical"] = vertical;

            dt.Rows.Add(dr);
        }

        /// <summary>
        /// method to handle the IDisposable interface contract
        /// </summary>
        public void Dispose()
        {
            WriteSkiRunsData();
            _skiRuns_ds.Dispose();
        }
    }
}
