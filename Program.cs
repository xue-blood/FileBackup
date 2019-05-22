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

        static ExProcess exp = new ExProcess();

        //static Func<string, string, List<string>, List<string>, string, string, string, string> run = Run;
        [Cmd]
        public static string Run([CmdParam(defaultValue = ".")]string from, string to,
            List<string> include, List<string> exclude,
            List<string> rename, List<string> remove,
            [CmdParam (isswitch = true)]string replace,
            [CmdParam (isswitch = true)]string debug ) {

            try {
                foreach (var p in Directory.GetFiles(from, "*.*", SearchOption.AllDirectories)) {
                    exp.init(p, from);
                    if (exp.isexclude(p, exclude)) goto __next__;
                    if (!exp.isinclude(p, include)) goto __next__;
                    
                    // new name
                    var np = p.Substring (from.Length + 1, p.Length - from.Length - 1);

                    np = exp.remove(np, remove);
                    np = exp.rename(np, rename);

                    np = Path.Combine(to, np);
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
