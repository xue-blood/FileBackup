using System;
using System.Collections.Generic;

namespace Command {
    public class CmdParam {

        /// <summary>
        /// 参数
        /// </summary>
        Dictionary<string, List<string>> paramDic = new Dictionary<string, List<string>> ();

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object data;

        /// <summary>
        /// 获取参数
        /// </summary>
        public string this[string key] {
            get {
                if (string.IsNullOrEmpty (key)) { return null; }
                if (paramDic.ContainsKey (key)) {
                    var list = paramDic[key];
                    if (list.valid ()) { return list[0]; }
                }
                return null;
            }
            set {
                if (string.IsNullOrEmpty (key)) { return; }
                if (!paramDic.ContainsKey (key)) {
                    paramDic.Add (key, new List<string> ());
                }
                paramDic[key].Add (value);
            }
        }

        /// <summary>
        /// 获取默认参数
        /// </summary>
        public string this[int _defaut] {
            get { return this["@"+_defaut]; }
            set { this["@"+_defaut] = value; }
        }

        /// <summary>
        /// 获取全部参数
        /// </summary>
        public List<string> this[string key, bool all] {
            get { return key != null && paramDic.ContainsKey (key) ? paramDic[key] : null; }
        }

        /// <summary>
        /// 获取全部默认参数
        /// </summary>
        public List<string> this[int _default, bool all] {
            get { return this["@0", all]; }
        }

        public string this[string longName, string shortName, int _default] {
            get { return this[tryFindValidKey (longName, shortName, _default)]; }
        }
        public List<string> this[string longName, string shortName, int _default, bool all] {
            get { return this[tryFindValidKey (longName, shortName, _default), all]; }
        }

        /// <summary>
        /// 尝试获取有效的参数名
        /// </summary>
        internal string tryFindValidKey ( string longName, string shortName, int _default ) {
            if (longName != null && paramDic.ContainsKey (longName)) { return longName; }
            if (shortName != null && paramDic.ContainsKey (shortName)) { return shortName; }
            if (paramDic.ContainsKey ("@"+_default)) { return "@"+_default; }
            return null;
        }

        public override string ToString () {
            string r = "";
            foreach (var p in paramDic) {
                r += string.Format ("[{0}:{1}] ", p.Key, getListString (p.Value));
            }
            return r.valid () ? r : "{}";
        }

        public void Clear () { paramDic.Clear (); }

        internal string getListString ( List<string> list, string split = "," ) {
            string r = "";
            if (list == null || list.Count == 0) { return r; }
            r = list[0];
            for (int i = 1; i<list.Count; i++) {
                r += split + list[i];
            }
            return r;
        }

        public List<string> getAllDefault () {
            List<string> r = new List<string> ();
            foreach (var p in paramDic) {
                if (p.Key.StartsWith ("@")) {
                    r.AddRange (p.Value);
                }
            }
            return r;
        }
    }
}
