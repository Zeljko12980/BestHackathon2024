using System.Diagnostics;
namespace API.Services
{
    public class PythonScriptService
    {
         public string RunPythonScript(string scriptPath, string args)
    {
        string result = string.Empty;
        
        try
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python"; // Može biti "python3" na nekim sistemima
            start.Arguments = $"{scriptPath} {args}"; // Putanja do skripte i argumenti (ako ih ima)
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true; // Da bi mogao da pročitaš izlaz iz skripte
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd(); // Čitaj izlaz iz Python skripte
                }
            }
        }
        catch (Exception ex)
        {
            result = $"Error: {ex.Message}";
        }

        return result;
    }
    }
}