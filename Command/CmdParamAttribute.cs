using System;

namespace Command {
    public class CmdParamAttribute : Attribute {
        public bool require { get; set; }       // 需要
        public object defaultValue { get; set; }// 默认值
        public bool isswitch { get; set; }      // 开关
    }
}
