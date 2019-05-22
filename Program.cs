using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Command;

namespace FileBackup {
    class Program {
        static void Main ( string[] args ) {
            //run.Method.Run<string> (args);
            typeof(Program).GetMethod("Run").Run<string>(args);
        }

        //static Func<string, string, List<string>, List<string>, string, string, string, string> run = Run;
        [Cmd]
        public static string Run([CmdParam(defaultValue = ".")]string from, string to,
            List<string> include, List<string> exclude,
            List<string> rename, List<string> remove,
            [CmdParam (isswitch = true)]string replace,
            [CmdParam (isswitch = true)]string debug ) {

            try {
                foreach (var p in Directory.GetFiles (from, "*.*", SearchOption.AllDirectories)) {
                    // is exclude
                    for (int i = 0; exclude != null && i<exclude.Count; i++) { if (p.IndexOf (exclude[i]) != -1) goto __next__; }
                    // is include
                    bool need = false;
                    for (int i = 0; include != null && i<include.Count; i++) { if (p.IndexOf (include[i]) != -1) { need = true; break; } }
                    if (!need) { goto __next__; }

                    // new name
                    var npb = new StringBuilder(p.Substring (from.Length + 1, p.Length - from.Length - 1));
                    
                    // need remove 
                    for (int i=0; remove != null && i < remove.Count; i++) { npb.Replace(remove[i], ""); }
                    // need rename file
                    for (int i=0; rename != null && i < rename.Count; i++) {
                        if (rename[i].Length == 2)
                            npb.Replace(rename[i][0], rename[i][1]); // just one character
                        else
                            npb.Replace(rename[i].Split(':')[0], rename[i].Split(':')[1]); // string replace
                    }

                    string np = Path.Combine(to, npb.ToString());
                    var nd = Path.GetDirectoryName (np);
                    Directory.CreateDirectory (nd);

                    if (File.Exists (np)) {
                        if (replace != null) { if (debug != null) { "Delete {0}".loge (np); }; File.Delete (np); }
                        else { goto __next__; }
                    }

                    File.Copy (p, np);

                    if (debug != null) { "Copy {0} \n\t{1}".logw (p, np); };
                    continue;
                    __next__:
                    if (debug != null) { "Skip {0}".log (p); };
                }
            }
            catch (Exception __e) {
                "Error {0}".loge (__e.Message);
                return __e.Message;
            }
            return "ok";
        }
    }
}
