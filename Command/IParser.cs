using System;
using System.Reflection;

namespace Command {
    public interface IParser {
        /// <summary>
        /// 分配参数到函数
        /// </summary>
        object[] getArgs ( MethodInfo info, CmdParam param, CmdAttribute attr );

        /// <summary>
        /// 解析参数
        /// </summary>
        CmdParam parseArgs ( string[] array );

        /// <summary>
        /// 解析参数
        /// </summary>
        CmdParam parseArgs ( string str, int startIndex = 0 );

        /// <summary>
        /// 反解析参数
        /// </summary>
        string unParser ( string str, int code = 0, object data = null );
    }
}
