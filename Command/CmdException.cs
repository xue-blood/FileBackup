using System;

namespace Command {
    class CmdException : Exception {
        public Code code;
        public string message;

        public CmdException ( Code code, string message ) {
            this.code = code;
            this.message = message;
        }

        public enum Code {
            success,    // 成功
            invalid,    // 无效
            paramMiss,  // 缺少参数
            outofRange, // 超出范围
            exception,  // 出現异常
            unsupport,  // 暂不支持
            update_pull,// 拉取失败
            update_merge,// 合并失败
            update_push, // 推送失败
        }
    }

    /// <summary>
    /// 用于保留日志
    /// </summary>
    class CmdLogException : Exception {
        public CmdLogException ( string message, Exception innerException = null ) : base (message, innerException) { }
    }
}
