using System;
using System.Collections.Specialized;
using System.IO;

namespace MTExample4_9 {
	class TextFileReader {
		public string[,] ReadTextFile (string fileName)
		{
			if (File.Exists(fileName)) {
				string[,] sArray = ReadFile(fileName);
				return sArray;
			}

			return null;
		}

		string[,] ReadFile (string fileName)
		{
			try {
				var sc = new StringCollection ();
				var fs = new FileStream (fileName, FileMode.Open, FileAccess.ReadWrite);
				var sr = new StreamReader (fs);

				// Read file into a string collection
				int noBytesRead = 0;
				string oneLine;
				while ((oneLine = sr.ReadLine()) != null) {
					noBytesRead += oneLine.Length;
					sc.Add (oneLine);
				}
				sr.Close ();

				string[] sArray = new string[sc.Count];
				sc.CopyTo (sArray, 0);

				char[] cSplitter = { ' ', ',', ':', '\t' };
				string[] sArray1 = sArray [0].Split (cSplitter);
				string[,] sArray2 = new string[sArray1.Length, sc.Count];

				for (int i = 0; i < sc.Count; i++) {
					sArray1 = sArray [sc.Count - 1 - i].Split (cSplitter);
					for (int j = 0; j < sArray1.Length; j++) {
						sArray2 [j, i] = sArray1 [j];
					}
				}
				return sArray2;
			} catch (Exception) {
				return null;
			}
		}
	}
}
