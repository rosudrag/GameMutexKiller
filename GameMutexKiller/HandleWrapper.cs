using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GameMutexKiller
{
	public static class HandleWrapper
	{
		private static string GetHandleExePath()
		{
			var currentExePath = Assembly.GetExecutingAssembly().Location;
			var currentDir = Path.GetDirectoryName(currentExePath);
			var handlePath = currentDir + "\\handle64.exe";
			return handlePath;
		}

		private static string RunHandleWithArgs(string args)
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = GetHandleExePath(),
					Arguments = args,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			process.Start();
			var output = process.StandardOutput.ReadToEnd();
			var err = process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (!string.IsNullOrEmpty(output)) return output;

			return err;
		}

		private static IEnumerable<Mutant> GetMutants(string mutantName)
		{
			var mutantStringResult = RunHandleWithArgs(string.Format("-a \"{0}\"", mutantName));

			var splitString = mutantStringResult.Split(
				new[] { Environment.NewLine },
				StringSplitOptions.None
			);

			var mutantStringLines = splitString.Where(x => x.Contains(".exe"));

			var mutantHexRegex = new Regex(@"\s.*(...): \\Sessions\\.*");
			var mutantPidRegex = new Regex("pid:\\s*(\\S*)\\s*type");

			foreach (var mutantStringLine in mutantStringLines)
			{
				var mutantHex = mutantHexRegex.Match(mutantStringLine).Groups[1].Value;
				var mutantPid = mutantPidRegex.Match(mutantStringLine).Groups[1].Value;

				yield return new Mutant() {PID = mutantPid, HEX = mutantHex};
			}
		}

		public static IEnumerable<string> KillLock(string mutantName)
		{
			var mutants = GetMutants(mutantName).ToList();
			foreach (var mutant in mutants)
			{
				yield return KillMutant(mutant);
			}
		}

		private static string KillMutant(Mutant mutant)
		{
			var args = string.Format("-p {0} -c {1} -y", mutant.PID, mutant.HEX);
			var result = RunHandleWithArgs(args);
			return result;
		}
	}
}