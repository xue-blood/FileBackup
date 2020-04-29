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
            [CmdParam(defaultValue = "")]string split,   // 1/5: div for 5 piece,  5 :  div 5 per piece
            [CmdParam(defaultValue = "")]string split_format,// {s} = src, {c} = current count, {p} = index or piece
            [CmdParam (isswitch = true)]string replace,
            [CmdParam (isswitch = true)]string debug ) {

            try {
                List<KeyValuePair<string, string>> passItem = new List<KeyValuePair<string, string>>();
                foreach (var p in Directory.GetFiles(from, "*.*", SearchOption.AllDirectories)) {
                    exp.init(p, from);
                    if (exp.isexclude(p, exclude)) goto __next__;
                    if (!exp.isinclude(p, include)) goto __next__;
                    
                    // new name
                    var np = p.Substring (from.Length + 1, p.Length - from.Length - 1);

                    np = exp.remove(np, remove);
                    np = exp.rename(np, rename);

                    passItem.Add(new KeyValuePair<string, string>(p, np));

                    if (debug != null) { "Copy {0} \n\t{1}".logw (p, np); };
                    continue;
                    __next__:
                    if (debug != null) { "Skip {0}".log (p); };
                }

                exp.init_split(split, passItem.Count, split_format);
                for(int i = 0; i < passItem.Count; i++)
                {
                    string np = Path.Combine(exp.outdir(to, i), passItem[i].Value);
                    var nd = Path.GetDirectoryName(np);
                    Directory.CreateDirectory(nd);

                    if (File.Exists(np))
                    {
                        if (replace != null) { if (debug != null) { "Delete {0}".loge(np); }; File.Delete(np); }
                    }

                    File.Copy(passItem[i].Key, np);
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
