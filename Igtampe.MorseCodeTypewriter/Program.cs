using Igtampe.BasicRender;
using Igtampe.MorseCodeTypewriter.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace Igtampe.MorseCodeTypewriter {
    class Program {

        public static Dictionary<char,string> TextToMorseCode;
        public static Dictionary<string,char> MorseCodeToText;

        static void Main(string[] args) {
            Init();
            Console.Clear();
            RenderUtils.Echo("MCODE Typer [Version 3.0]" +
                          "\n(C)2020 Igtampe, No rights reserved\n\nType '/LIVE' to enter LIVE typing mode, then hit ESCAPE to exit");

            if(args.Length == 1) {if(File.Exists(args[0])) { ConvertLines(File.ReadAllLines(args[0])); }}

            while(true) {

                RenderUtils.Echo("\n\n\n:");

                //save pos
                int CurLeft = Console.CursorLeft;
                int CurTop = Console.CursorTop;

                //Echo the text
                string Line = Console.ReadLine();
                if(Line.ToUpper() == "/LIVE") { LiveMode(); } else if(Line.ToUpper() == "EXIT") { break; }
                else { 
                    //Set the pos back
                    RenderUtils.SetPos(CurLeft,CurTop);
                    ConvertString(Line);
                }
            }
        }

        public static void LiveMode() {
            RenderUtils.Echo("\n\n");
            Draw.Sprite("Live typing mode engaged",ConsoleColor.Black,ConsoleColor.Green);
            RenderUtils.Echo("\n\n\n:");

            ConsoleKeyInfo Key = Console.ReadKey(false);
            while(Key.Key != ConsoleKey.Escape) {
                if(TextToMorseCode.ContainsKey(char.ToUpper(Key.KeyChar))) {
                    Console.CursorLeft = Math.Max(Console.CursorLeft - 1,0);
                    Type(TextToMorseCode[char.ToUpper(Key.KeyChar)]);
                }
                if(Key.Key == ConsoleKey.Enter) { RenderUtils.Echo("\n\n\n:"); }
                Key = Console.ReadKey(false);
            }

            RenderUtils.Echo("\n\n");
            Draw.Sprite("LLive typing mode disengaged",ConsoleColor.Black,ConsoleColor.Red);

        }


        /// <summary>Initializes the morse code dictionary</summary>
        public static void Init() {
            TextToMorseCode = new Dictionary<char,string>(40) {
                { 'A',".-"    },
                { 'B',"-..."  },
                { 'C',"-.-."  },
                { 'D',"-.."   },
                { 'E',"."     },
                { 'F',"..-."  },
                { 'G',"--."   },
                { 'H',"...."  },
                { 'I',".."    },
                { 'J',".---"  },
                { 'K',"-.-"   },
                { 'L',".-.."  },
                { 'M',"--"    },
                { 'N',"-."    },
                { 'O',"---"   },
                { 'P',".--."  },
                { 'Q',"--.-"  },
                { 'R',".-."   },
                { 'S',"..."   },
                { 'T',"-"     },
                { 'U',"..-"   },
                { 'V',"...-"  },
                { 'W',".--"   },
                { 'X',"-..-"  },
                { 'Y',"-.--"  },
                { 'Z',"--.."  },
                { '1',".----" },
                { '2',"..---" },
                { '3',"...--" },
                { '4',"....-" },
                { '5',"....." },
                { '6',"-...." },
                { '7',"--..." },
                { '8',"---.." },
                { '9',"----." },
                { '0',"-----" },
                { '.',"+"     },
                { ' ',"/"     },
                { '@',"?"     }
            };
            MorseCodeToText = new Dictionary<string,char>(40) {
                { ".-"    ,'A'},
                { "-..."  ,'B'},
                { "-.-."  ,'C'},
                { "-.."   ,'D'},
                { "."     ,'E'},
                { "..-."  ,'F'},
                { "--."   ,'G'},
                { "...."  ,'H'},
                { ".."    ,'I'},
                { ".---"  ,'J'},
                { "-.-"   ,'K'},
                { ".-.."  ,'L'},
                { "--"    ,'M'},
                { "-."    ,'N'},
                { "---"   ,'O'},
                { ".--."  ,'P'},
                { "--.-"  ,'Q'},
                { ".-."   ,'R'},
                { "..."   ,'S'},
                { "-"     ,'T'},
                { "..-"   ,'U'},
                { "...-"  ,'V'},
                { ".--"   ,'W'},
                { "-..-"  ,'X'},
                { "-.--"  ,'Y'},
                { "--.."  ,'Z'},
                { ".----" ,'1'},
                { "..---" ,'2'},
                { "...--" ,'3'},
                { "....-" ,'4'},
                { "....." ,'5'},
                { "-...." ,'6'},
                { "--..."  ,'7'},
                { "---.." ,'8'},
                { "----." ,'9'},
                { "-----" ,'0'},
                { "+"     ,'.'},
                { "/"     ,' '},
                { "?"     ,'@'}
            };

        }

        public static void ConvertLines(String[] lines) {
            foreach(String line in lines) {
                RenderUtils.Echo("\n\n\n:");

                //save pos
                int CurLeft = Console.CursorLeft;
                int CurTop = Console.CursorTop;

                //Echo the text
                RenderUtils.Echo(line);

                //Set the pos back
                RenderUtils.SetPos(CurLeft,CurTop);

                ConvertString(line);
            }
        }

        public static void ConvertString(String line) {
            //Convert
            if(line.StartsWith('.') || line.StartsWith('-')) {
                //assume the given text is morse code. Reverse!
                foreach(string bit in line.Split(' ')) { if(MorseCodeToText.ContainsKey(bit)) { Console.Write(MorseCodeToText[bit]); } }
            } else { foreach(char C in line) { if(TextToMorseCode.ContainsKey(char.ToUpper(C))) { Type(TextToMorseCode[char.ToUpper(C)]); } } }
        }

        /// <summary>Types out a morse code string (along with beeps)</summary>
        /// <param name="Beep"></param>
        public static void Type(string Beep) {
            foreach(char bit in Beep) {
                Console.Write(bit);
                switch(bit) {
                    case '.':
                        new SoundPlayer(Resources.dot).PlaySync();
                        break;
                    case '-':
                        new SoundPlayer(Resources.dash).PlaySync();
                        break;
                    case '/':
                        Thread.Sleep(100);
                        break;
                    case '+':
                        Thread.Sleep(500);
                        break;
                    default: break;
                }
            }
            Console.Write(' ');
        }

    }
}
