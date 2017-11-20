using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiRunRater
{
    public class Controller
    {
        #region ENUMERABLES


        #endregion

        #region FIELDS

        bool active = true;

        #endregion

        #region PROPERTIES


        #endregion

        #region CONSTRUCTORS

        public Controller()
        {
            ApplicationControl();
        }

        #endregion

        #region METHODS

        private void ApplicationControl()
        {
            ConsoleView.DisplayWelcomeScreen();

            while (active)
            {
                AppEnum.ManagerAction userActionChoice;

                userActionChoice = ConsoleView.GetUserActionChoice();

                switch (userActionChoice)
                {
                    case AppEnum.ManagerAction.None:
                        break;

                    case AppEnum.ManagerAction.ListAllSkiRuns:
                        ListAllSkiRuns();
                        break;

                    case AppEnum.ManagerAction.DisplaySkiRunDetail:
                        DisplaySkiRunDetail();
                        break;

                    case AppEnum.ManagerAction.AddSkiRun:
                        AddSkiRun();
                        break;

                    case AppEnum.ManagerAction.UpdateSkiRun:
                        UpdateSkiRun();
                        break;

                    case AppEnum.ManagerAction.DeleteSkiRun:
                        DeleteSkiRun();
                        break;

                    case AppEnum.ManagerAction.QuerySkiRunsByVertical:
                        QuerySkiRunsByVertical();
                        break;

                    case AppEnum.ManagerAction.Quit:
                        active = false;
                        break;

                    default:
                        break;
                }
            }
            ConsoleView.DisplayExitPrompt();
        }

        private static void ListAllSkiRuns()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            List<SkiRun> skiRuns;

            using (skiRunRepository)
            {
                skiRuns = skiRunRepository.SelectAllRuns();
            }

            ConsoleView.DisplayAllSkiRuns(skiRuns);

            ConsoleView.DisplayContinuePrompt();
        }

        private static void DisplaySkiRunDetail()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            List<SkiRun> skiRuns = skiRunRepository.SelectAllRuns();
            SkiRun skiRun = new SkiRun();
            int skiRunID;

            using (skiRunRepository)
            {
                skiRuns = skiRunRepository.SelectAllRuns();
            }

            skiRunID = ConsoleView.GetSkiRunID(skiRuns);

            using (skiRunRepository)
            {
                skiRun = skiRunRepository.SelectByID(skiRunID);
            }

            ConsoleView.DisplaySkiRun(skiRun);
            ConsoleView.DisplayContinuePrompt();
        }

        private static void AddSkiRun()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            SkiRun skiRun = new SkiRun();

            skiRun = ConsoleView.AddSkiRun();
            using (skiRunRepository)
            {
                skiRunRepository.InsertSkiRun(skiRun);
            }

            ConsoleView.DisplayContinuePrompt();
        }

        private static void UpdateSkiRun()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            List<SkiRun> skiRuns = skiRunRepository.SelectAllRuns();
            SkiRun skiRun = new SkiRun();
            int skiRunID;

            using (skiRunRepository)
            {
                skiRuns = skiRunRepository.SelectAllRuns();
            }

            skiRunID = ConsoleView.GetSkiRunID(skiRuns);

            using (skiRunRepository)
            {
                skiRun = skiRunRepository.SelectByID(skiRunID);
            }

            skiRun = ConsoleView.UpdateSkiRun(skiRun);

            using (skiRunRepository)
            {
                skiRunRepository.UpdateSkiRun(skiRun);
            }
        }

        private static void DeleteSkiRun()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            List<SkiRun> skiRuns = skiRunRepository.SelectAllRuns();
            SkiRun skiRun = new SkiRun();
            int skiRunID;
            string message;

            skiRunID = ConsoleView.GetSkiRunID(skiRuns);

            using (skiRunRepository)
            {
                skiRunRepository.DeleteSkiRun(skiRunID);
            }

            ConsoleView.DisplayReset();

            // TODO refactor
            message = String.Format("Ski Run ID: {0} had been deleted.", skiRunID);

            ConsoleView.DisplayMessage(message);
            ConsoleView.DisplayContinuePrompt();
        }

        private static void QuerySkiRunsByVertical()
        {
            SkiRunRepositoryXML_DS skiRunRepository = new SkiRunRepositoryXML_DS();
            List<SkiRun> matchingSkiRuns = new List<SkiRun>();
            int minimumVertical;
            int maximumVertical;

            ConsoleView.GetVerticalQueryMinMaxValues(out minimumVertical, out maximumVertical);

            using (skiRunRepository)
            {
                matchingSkiRuns = skiRunRepository.QueryByVertical(minimumVertical, maximumVertical);
            }

            ConsoleView.DisplayQueryResults(matchingSkiRuns);
            ConsoleView.DisplayContinuePrompt();
        }

        #endregion

    }
}
