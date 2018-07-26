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
            run.Method.Run<string> (args);
        }

        static Func<string, string, List<string>, List<string>, string, string, string, string> run = Run;
        [Cmd]
        public static string Run ( string from, string to, List<string> include, List<string> exclude, string rename,
            [CmdParam (isswitch = true)]string replace,
            [CmdParam (isswitch = true)]string debug ) {

            try {
                foreach (var p in Directory.GetFiles (from, "*.*", SearchOption.AllDirectories)) {
                    // 是否被包含
                    bool need = false;
                    for (int i = 0; include != null && i<include.Count; i++) { if (p.IndexOf (include[i]) != -1) { need = true; break; } }
                    if (!need) { goto __next__; }
                    for (int i = 0; exclude != null && i<exclude.Count; i++) { if (p.IndexOf (exclude[i]) != -1) goto __next__; }

                    var np = p.Substring (from.Length + 1, p.Length - from.Length - 1);
                    if (rename != null && rename.Length == 2) { np = np.Replace (rename[0], rename[1]); }
                    np = Path.Combine (to, np);
                    var nd = Path.GetDirectoryName (np);
                    Directory.CreateDirectory (nd);

                    if (File.Exists (np)) {
                        if (replace != null) { if (debug != null) { "Delete {0}".log (np); }; File.Delete (np); }
                        else { goto __next__; }
                    }

                    File.Copy (p, np);

                    if (debug != null) { "Copy {0} \n\t{1}".log (p, np); };
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
