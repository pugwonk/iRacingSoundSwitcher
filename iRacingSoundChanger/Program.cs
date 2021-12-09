using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace iRacingSoundChanger
{
    class Program
    {

        private static string iniGetValue(string[] inif, string name)
        {
            for (int i = 0; i < inif.Length; i++)
            {
                string prefix = name + "=";
                if (inif[i].StartsWith(prefix))
                {
                    string outp = Regex.Replace(inif[i].Substring(prefix.Length), @"\;.*", "").Trim();
                    return outp;
                }
            }
            throw new Exception($"Can't find INI file entry for {name}");
        }

        private static void iniSetValue(string[] inif, string name, string newVal)
        {
            for (int i = 0; i < inif.Length; i++)
            {
                string prefix = name + "=";
                if (inif[i].StartsWith(prefix))
                {
                    inif[i] = prefix + newVal;
                }
            }
        }

        static void Main(string[] args)
        {
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var iRacingDocs = Path.Combine(docs, "iRacing");
            string appIni = Path.Combine(iRacingDocs, "app.ini");
            string appIniBak = Path.Combine(iRacingDocs, "app.ini.bak");
            if (!File.Exists(appIniBak))
            {
                File.Copy(appIni, appIniBak, true);
                Console.WriteLine($"Saving current INI file as {appIniBak}");
            }

            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a parameter with the name to save the current settings as (e.g. \"vr\")");
            }

            string[] iniContents = File.ReadAllLines(appIni);
            string devSpeakerId = iniGetValue(iniContents, "devSpeakerId");
            string devSpeakerName = iniGetValue(iniContents, "devSpeakerName");
            string devVoiceChatId = iniGetValue(iniContents, "devVoiceChatId");
            string devVoiceChatName = iniGetValue(iniContents, "devVoiceChatName");

            if (args.Length == 1)
            {
                string mypath = AppDomain.CurrentDomain.BaseDirectory;
                string batch = $"\"{mypath}iRacingSoundChanger\" {devSpeakerId} \"{devSpeakerName}\" {devVoiceChatId} \"{devVoiceChatName}\"";
                string fn = args[0] + ".bat";
                File.WriteAllText(fn, batch);
                Console.WriteLine($"Current sound settings saved as {fn}");
            }

            if (args.Length == 4)
            {
                Console.WriteLine("Setting sound devices.");
                iniSetValue(iniContents, "devSpeakerId", args[0]);
                iniSetValue(iniContents, "devSpeakerName", args[1]);
                iniSetValue(iniContents, "devVoiceChatId", args[2]);
                iniSetValue(iniContents, "devVoiceChatName", args[3]);
                File.WriteAllLines(appIni, iniContents);
            }
        }
    }
}
