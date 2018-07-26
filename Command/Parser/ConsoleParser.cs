using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Command.Parser {
    class ConsoleParser : IParser {
        public object[] getArgs ( MethodInfo info, CmdParam param, CmdAttribute attr ) {
            var fparam = info.GetParameters ();
            object[] args = new object[fparam.Length];

            if (attr == null) {
                Debug.Fail ("Must apply a [MsgAttribute] to method");
                return args;
            }
            // 沒有参数
            if (fparam.Length == 0) {
                return args;
            }

            // 多个参数
            for (int i = 0; i<fparam.Length; i++) {
                var f = fparam[i];
                var l = f.Name.ToLower ();  // 长名字
                var s = !attr.disableShort ? l[0].ToString () : null;   // 短名字
                var d = attr.collectDefault == null && s != null ? i : -1;

                // 单个参数
                if (f.ParameterType == typeof (string))
                    args[i] = param[l, s, d];
                // 多个参数
                else if (f.ParameterType == typeof (List<string>))
                    args[i] = param[l, s, d, true];
                else
                    System.Console.WriteLine (string.Format ("Error parameter type : {0} {1}", f.Name, f.ParameterType));

                // 收集默认参数
                if (attr.collectDefault == f.Name.ToLower ()) {
                    if (f.ParameterType == typeof (List<string>)) {
                        var all = param.getAllDefault ();
                        if (args[i]!= null) { all.AddRange ((List<string>)args[i]); }
                        args[i] = all;
                    }
                    else
                        System.Console.WriteLine (string.Format ("Parameter type must be List<string>"));
                }

                var mpa = f.GetCustomAttribute<CmdParamAttribute> ();
                // 设置了属性
                if (mpa != null) {
                    if (args[i] == null) {
                        if (mpa.require) { throw new ArgumentException ("Require parameter : {0}!".format (l)); }// 需要的参数
                        else if (mpa.defaultValue != null) { args[i] = mpa.defaultValue; }// 默认值
                        else if (mpa.isswitch) { args[i] = "true"; } // 开关
                    }
                }
            }

            return args;
        }

        protected string longPre = "--";
        protected string shortPre = "-";

        public CmdParam parseArgs ( string[] array ) {
            CmdParam param = new CmdParam ();
            int @default = 0;
            for (int i = 0; i<array.Length; i++) {
                if (string.IsNullOrEmpty (array[i])) { continue; }

                // 不是参数
                if (!array[i].StartsWith (longPre) && !array[i].StartsWith (shortPre)) {
                    param[@default++] = array[i];
                }
                // 是否是最后一个
                else if (i == array.Length -1) {
                    // 长参数
                    if (array[i].StartsWith (longPre))
                        param[array[i].Replace (longPre, "")] = null;
                    // 短参数
                    else
                        param[array[i].Replace (shortPre, "")] = null;
                }
                else {
                    // 下一个不是参数
                    if (!array[i + 1].StartsWith (longPre) && !array[i + 1].StartsWith (shortPre)) {
                        // 长参数
                        if (array[i].StartsWith (longPre))
                            param[array[i].Replace (longPre, "")] = array[i + 1];
                        // 短参数
                        else
                            param[array[i].Replace (shortPre, "")] = array[i + 1];
                        i++;
                    }
                    else {
                        // 长参数
                        if (array[i].StartsWith (longPre))
                            param[array[i].Replace (longPre, "")] = null;
                        // 短参数
                        else
                            param[array[i].Replace (shortPre, "")] = null;
                    }
                }
            }
#if DEBUG
            System.Console.WriteLine (param.ToString ());
#endif
            return param;
        }

        public CmdParam parseArgs ( string str, int startIndex = 0 ) {
            List<StringBuilder> sbds = new List<StringBuilder> ();
            sbds.Add (new StringBuilder ());
            bool quata = false; // 是否处于引号中

            for (int i = startIndex; i<str.Length; i++) {
                switch (str[i]) {
                    case ' ':
                        if (!quata)
                            sbds.Add (new StringBuilder ());
                        else
                            goto default;
                        break;
                    case '"':
                        quata = !quata;
                        break;
                    default:
                        sbds[sbds.Count -1].Append (str[i]);
                        break;
                }
            }


            string[] strs = new string[sbds.Count];
            for (int i = 0; i<strs.Length; i++) { strs[i] = sbds[i].ToString (); }

            return parseArgs (strs);
        }

        public string unParser ( string str, int code = 0, object data = null ) {
            throw new NotImplementedException ();
        }
    }
}
