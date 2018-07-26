using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Command.Parser {
    class FileParser : IParser {
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

            for (int i = 0; i<fparam.Length; i++) {
                var f = fparam[i];
                var name = fparam[i].Name;
                // 单个参数
                if (f.ParameterType == typeof (string))
                    args[i] = param[name];
                // 多个参数
                else if (f.ParameterType == typeof (List<string>))
                    args[i] = param[name, true];
                else
                    System.Console.WriteLine (string.Format ("Error parameter type : {0} {1}", f.Name, f.ParameterType));

                var mpa = f.GetCustomAttribute<CmdParamAttribute> ();
                // 设置了属性
                if (mpa != null) {
                    if (args[i] == null) {
                        if (mpa.require) { throw new ArgumentException ("Require parameter : {0}!".format (name)); }// 需要的参数
                        else if (mpa.defaultValue != null) { args[i] = mpa.defaultValue; }// 默认值
                    }
                    else {
                        if (mpa.isswitch) { args[i] = "true"; } // 开关
                    }
                }
            }

            return args;
        }

        public CmdParam parseArgs ( string[] array ) {
            CmdParam param = new CmdParam ();
            if (array == null) { return param; }
            for (int i = 0; i<array.Length; i++) {
                var raw = array[i].Trim ();
                if (!raw.valid () || raw[0] =='#') { continue; }
                string[] one = raw.Split ('=');
                if (one.Length == 1) {
                    param[one[0]] = null;
                }
                else if (one.Length == 2) {
                    param[one[0]] = one[1];
                }
            }
            return param;
        }

        public CmdParam parseArgs ( string str, int startIndex = 0 ) {
            if (!File.Exists (str)) {
                Console.WriteLine ("File no found: "+str);
                return new CmdParam ();
            }
            return parseArgs (File.ReadAllLines (str));
        }

        public string unParser ( string str, int code = 0, object data = null ) {
            throw new NotImplementedException ();
        }
    }
}
