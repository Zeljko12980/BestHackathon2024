using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace API.Services
{
    public class PythonScriptService
    {
        public async Task<string> RecognizeFaceAsync()
        {
            string result = string.Empty;

            // Putanja do Python skripte (možemo koristiti apsolutnu ili relativnu putanju)
             string scriptPath = @"C:\Users\ikano\source\repos\Zeljko12980\BestHackathon2024\API\skripta\AI Face Recgn\main.py";  // Putanja do skripte (možeš promeniti putanju)


  if (!File.Exists(scriptPath))
            {
                return "Python skripta nije pronađena.";
            }
            try
            {
                // Pokretanje Python skripte iz fajla
                result = await RunPythonScriptAsync(scriptPath);
            }
            catch (Exception ex)
            {
                result = $"Error: {ex.Message}";
            }

            return result;
        }

        private async Task<string> RunPythonScriptAsync(string scriptPath)
        {
            string result = string.Empty;

            try
            {
                // Pokretanje Python skripte u novom procesu
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python"; // Može biti "python3" na nekim sistemima
                start.Arguments = scriptPath; // Pokreće Python fajl
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true; // Da bi mogao da pročitaš izlaz iz skripte
                start.RedirectStandardError = true; // Da bismo uhvatili greške
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = await reader.ReadToEndAsync(); // Čitaj izlaz iz Python skripte
                    }

                    using (StreamReader errorReader = process.StandardError)
                    {
                        string error = await errorReader.ReadToEndAsync();
                        if (!string.IsNullOrEmpty(error))
                        {
                            result = $"Error: {error}";
                        }
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
