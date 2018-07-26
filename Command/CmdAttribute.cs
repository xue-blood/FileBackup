using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command {
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false)]
    public class CmdAttribute : Attribute {
        public string format { get; set; }  // 格式
        public string descript { get; set; }// 描述
        public bool disable { get; set; }   // 禁用
        public bool disableShort { get; set; }  // 禁用短名字
        public string collectDefault { get; set; }  // 收集默认参数
        [DebuggerStepThrough]
        public CmdAttribute ( string descript = null, string format = null ) {
            this.format = format;
            this.descript = descript;
        }
    }
}
