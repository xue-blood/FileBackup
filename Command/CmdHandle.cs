using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Command {
    public class CmdHandle {
        static Dictionary<string, IParser> IParsers;
        static CmdHandle () {
            IParsers = new Dictionary<string, IParser> ();
            IParsers.Add ("Console", new Parser.ConsoleParser ());
            IParsers.Add ("File", new Parser.FileParser ());
        }

        public static void loadMethods<T> ( Dictionary<string, MethodInfo> dic ) {
            Debug.WriteLine ("[MsgHandle]");
            foreach (var f in typeof (T).GetMethods ()) {
                addMethod (dic, f);
            }
        }
        static internal void addMethod ( Dictionary<string, MethodInfo> dic, MethodInfo info ) {
            if (dic == null || info == null) { return; }
            var a = info.GetCustomAttribute<CmdAttribute> ();
            if (a != null && !a.disable && info.Name.StartsWith ("Cmd")) {
                // 名字
                var name = info.Name.ToLower ().Substring (3);
                // 是否已经存在
                if (dic.ContainsKey (name)) {
                    Debug.WriteLine ("MsgHandle exisited : "+name);
                }
                else {
                    dic.Add (name, info);
                    Debug.WriteLine ("\t{0}\t{1}{3}\t{2}".format (name, a.format, a.descript,
                        a.format.valid () ? "" : "\t"));
                }
            }
        }

        IParser iparser;


        /// <summary>
        /// 命令
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public CmdParam param = new CmdParam ();



        public void Read ( string arg, int startIndex = 0, string parser = "Console" ) {
            iparser = IParsers[parser];
            param.Clear ();
            param = iparser.parseArgs (arg, startIndex);
        }

        public void Read ( string[] args, string parser = "Console" ) {
            iparser = IParsers[parser];
            param.Clear ();
            param = iparser.parseArgs (args);
        }

        public string Write ( string msg, int code = 0, string parser = "Console" ) {
            iparser = IParsers[parser];
            return iparser.unParser (msg, code, param.data);
        }

        public T Run<T> ( Dictionary<string, MethodInfo> dic ) {
            if (!dic.ContainsKey (name)) { throw new CmdException (CmdException.Code.invalid, "无效命令"); }
            return Run<T> (dic[name]);
        }

        public T Run<T> ( MethodInfo info ) {
            try {
                var args = iparser.getArgs (info, param, info.GetCustomAttribute<CmdAttribute> ());
                try { return (T)info.Invoke (null, args); }
                catch (TargetInvocationException te) { throw te.InnerException; }
            }
            catch (ArgumentOutOfRangeException aoe) {
                throw new CmdException (CmdException.Code.outofRange, aoe.Message);
            }
            catch (ArgumentException ae) { throw new CmdException (CmdException.Code.paramMiss, ae.Message); }
        }
    }
}
