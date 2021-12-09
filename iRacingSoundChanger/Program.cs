using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

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

        static void Main(string[] args)
        {
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var iRacingDocs = Path.Combine(docs, "iRacing"); 
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                string appIni = Path.Combine(iRacingDocs, "app.ini");
                string appIniBak = Path.Combine(iRacingDocs, "app.ini.bak");
                Console.WriteLine($"Saving current INI file as {appIniBak}");
                File.Copy(appIni, appIniBak, true);

                //var pconf = new IniData();
                //pconf.Configuration.AssigmentSpacer = "";
                var parser = new FileIniDataParser(new IniDataParser(new IniParserConfiguration() { AssigmentSpacer = "" }));
                IniData data = parser.ReadFile(appIni);
                if (o.OutputMain != null)
                {
                    string curMainD = data["Audio"]["devSpeakerName"];
                    // Sometimes this ends in comments
                    curMainD = Regex.Replace(curMainD, @"\;.*", "").Trim();
                    Console.WriteLine($"Changing main audio device from {curMainD} to {o.OutputMain}");
                    data["Audio"]["devSpeakerName"] = o.OutputMain;
                }

                if (o.OutputVoice != null)
                {
                    string curVoiceD = data["Audio"]["devVoiceChatName"];
                    // Sometimes this ends in comments
                    curVoiceD = Regex.Replace(curVoiceD, @"\;.*", "").Trim();
                    Console.WriteLine($"Changing voice audio device from {curVoiceD} to {o.OutputVoice}");
                    data["Audio"]["devVoiceChatName"] = o.OutputVoice;
                }
                parser.WriteFile(appIni, data);
            });
        }
    }
}
