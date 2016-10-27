namespace AppQuest_Schatzkarte.Services
{
    public interface ILogBuchService
    {
        void OpenLogBuch(string task, string solution, string solutionName = "solution");
    }
}