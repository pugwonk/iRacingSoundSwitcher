using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using CommandLine;

namespace iRacingSoundChanger
{
    class Program
    {

        public class Options
        {
            [Option('m', Required = false, HelpText = "Main output device e.g. -m \"System Default\"")]
            public string OutputMain { get; set; }
            [Option('v', Required = false, HelpText = "Voice output device e.g. -v \"Headphones (2- Rift S)\"")]
            public string OutputVoice { get; set; }
        }

        private static string iniGetValue(string[] inif, string name)
        {
            for (int i = 0; i < inif.Length; i++)
            {
                string prefix = name + "=";
                if (inif[i].StartsWith(prefix))
                {
                    return inif[i].Substring(prefix.Length);
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
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                string appIni = Path.Combine(iRacingDocs, "app.ini");
                string appIniBak = Path.Combine(iRacingDocs, "app.ini.bak");
                Console.WriteLine($"Saving current INI file as {appIniBak}. To use the current main audio and voice audio, type:");
                Console.WriteLine();

                string[] iniContents = File.ReadAllLines(appIni);
                string curMainD = iniGetValue(iniContents, "devSpeakerId");
                curMainD = Regex.Replace(curMainD, @"\;.*", "").Trim();
                string curVoiceD = iniGetValue(iniContents, "devVoiceChatId");
                curVoiceD = Regex.Replace(curVoiceD, @"\;.*", "").Trim();
                Console.WriteLine($"iRacingSoundChanger -m {curMainD} -v {curVoiceD}");

                File.Copy(appIni, appIniBak, true);

                if (o.OutputMain != null)
                {
                    // Sometimes this ends in comments
                    Console.WriteLine($"Setting main audio device to {o.OutputMain}");
                    iniSetValue(iniContents, "devSpeakerId", o.OutputMain);
                    iniSetValue(iniContents, "devSpeakerName", "tbd");
                }

                if (o.OutputVoice != null)
                {
                    // Sometimes this ends in comments
                    Console.WriteLine($"Setting voice audio device to {o.OutputVoice}");
                    iniSetValue(iniContents, "devVoiceChatId", o.OutputVoice);
                    iniSetValue(iniContents, "devVoiceChatName", "tbd");
                }
                File.WriteAllLines(appIni, iniContents);
            });
        }
    }
}
