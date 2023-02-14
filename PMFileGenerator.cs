using SRT;
using System.Text;

namespace PMFileGenerator
{
    internal class PMFileGenerator
    {
        static void Main(string[] args)
        {
            PMFileGenerator p = new PMFileGenerator();
            p.RealMain(args);
        }

        private void RealMain(string[] args)
        {
            Console.WriteLine("PMFileGenerator version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("");

            if(args.Length != 2)
            {
                DisplayHelp();
                return;
            }

            string srtFile = args[0];
            string pottyFile = args[1];
            string outSrtFile = Path.ChangeExtension(srtFile, ".muted.srt");

            // Emby SRT file can have a language identifier like  ,eng, but need to get rid of that for EDL file
            string test1 = Path.GetFileNameWithoutExtension(srtFile);
            string outEdlFile = Path.ChangeExtension(test1, ".edl");
           
            Console.WriteLine($"SRT File: {srtFile}");
            Console.WriteLine($"Potty Words File: {pottyFile}");
            Console.WriteLine($"EDL File: {outEdlFile}");
            Console.WriteLine($"Muted SRT File: {outSrtFile}");
            Console.WriteLine("");

            if(!File.Exists(srtFile))
            {
                Console.WriteLine($"SRT File {srtFile} does not exist.");
                return;
            }

            List<string> swearWords = new List<string>(File.ReadAllLines(pottyFile));

            StringBuilder edlSb = new StringBuilder();
            SRTFile srtIn = new SRTFile(srtFile);
            List<Subtitle> subsOut = new List<Subtitle>();
            int outIndex = 1;
            bool found = false;
            List<string> newLines = new List<string>();

            foreach (Subtitle st in srtIn.Subtitles)
            {
                foreach(string line in st.Lines)
                {
                    string theLine = line;

                    foreach(string swear in swearWords)
                    {
                        if(IndexOfWholeWord(line.ToLower(), swear.ToLower()) != -1)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < swear.Length; i++)
                                sb.Append("*");

                            theLine = theLine.Replace(swear, sb.ToString(), StringComparison.OrdinalIgnoreCase);

                            found = true;
                        }
                    }

                    newLines.Add(theLine);  
                }

                if (found)
                {
                    string edlLine = ((double)st.StartTime.TotalMilliseconds / (double)1000).ToString("0.00") + "\t" + ((double)st.EndTime.TotalMilliseconds / (double)1000).ToString("0.00") + "\t" + "1";
                    edlSb.AppendLine(edlLine);

                    Subtitle newSt = new Subtitle(outIndex++);
                    newSt.StartTime = st.StartTime;
                    newSt.EndTime = st.EndTime;

                    foreach(string s in newLines)
                        newSt.Lines.Add(s);

                    subsOut.Add(newSt);
                    found = false;                 
                }

                newLines.Clear();
            }

            using (TextWriter tw = new StreamWriter(outEdlFile, false))
            {
                tw.WriteLine(edlSb.ToString());
            }

            using (TextWriter tw = new StreamWriter(outSrtFile, false))
            {
                StringBuilder sbOut = new StringBuilder();

                for (int i = 0; i < subsOut.Count; i++)
                    sbOut.Append(subsOut[i].ToString());

                tw.WriteLine(sbOut.ToString());
            }
        }

        /// <summary>
        /// Find whole word matches.  Required so that a muted word of "ass" doesn't get flagged in the word "assemble".
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public int IndexOfWholeWord(string str, string word)
        {
            for (int j = 0; j < str.Length &&
                (j = str.IndexOf(word, j, StringComparison.Ordinal)) >= 0; j++)
                if ((j == 0 || !char.IsLetterOrDigit(str, j - 1)) &&
                    (j + word.Length == str.Length || !char.IsLetterOrDigit(str, j + word.Length)))
                    return j;

            return -1;
        }

        private void DisplayHelp()
        {
            Console.WriteLine("PMFileGenerator <Full path to srt file> <Full path to muted words file>");
            Console.WriteLine("EDL and muted SRT files created in the same directory as the SRT file.");
            Console.WriteLine("");

            Console.WriteLine("Hit enter to continue");
            Console.ReadLine();
        }
    }
}