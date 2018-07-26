using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// 字符串扩展
/// </summary>
public static class StringExtension {

    /// <summary>
    /// 按错误打印
    /// </summary>
    /// <param name="str">要打印的字符串，可以帶格式化</param>
    /// <param name="param">格式化参数</param>
    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough ()]
    public static void loge ( this string str, params object[] args ) {
#if UNITY_EDITOR
        Debug.LogError (string.Format (str, args));
#elif true
        System.Console.ForegroundColor = System.ConsoleColor.Red;
        System.Console.WriteLine (string.Format (str, args));
        System.Console.ResetColor ();
#endif
    }


    /// <summary>
    /// 按警告打印
    /// </summary>
    /// <param name="str">要打印的字符串，可以帶格式化</param>
    /// <param name="param">格式化参数</param>
    [System.Diagnostics.DebuggerHidden]
    public static void logw ( this string str, params object[] args ) {
#if UNITY_EDITOR
        Debug.LogWarning (string.Format (str, args));
#elif true
        System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
        System.Console.WriteLine (string.Format (str, args));
        System.Console.ResetColor ();
#endif
    }

    /// <summary>
    /// 普通打印
    /// </summary>
    /// <param name="str">要打印的字符串，可以帶格式化</param>
    /// <param name="param">格式化参数</param>
    [System.Diagnostics.DebuggerHidden]
    public static void log ( this string str, params object[] args ) {
#if UNITY_EDITOR
        Debug.Log (string.Format (str, args));
#elif true
        System.Console.ForegroundColor = System.ConsoleColor.DarkGreen;
        System.Console.WriteLine (string.Format (str, args));
        System.Console.ResetColor ();
#endif
    }

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <param name="param">参数</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static string format ( this string str, params object[] param ) {
        return string.Format (str, param);
    }

    #region 格式转换
    /// <summary>
    /// 字符串转换为整型
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static int toInt ( this string str ) {
        if (str == null) { return 0; }
        int result = 0;
        int i = 0;
        for (; i<str.Length && !char.IsDigit (str[i]); i++)
            ;
        for (; i < str.Length&& char.IsDigit (str[i]); i++)
            result = 10 * result + (str[i] - 48);
        return result;
    }

    /// <summary>
    /// 字符串转换为長整型
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static long toLong ( this string str ) {
        if (str == null) { return 0; }
        long result = 0;
        int i = 0;
        for (; i<str.Length && !char.IsDigit (str[i]); i++)
            ;
        for (; i < str.Length&& char.IsDigit (str[i]); i++)
            result = 10 * result + (str[i] - 48);
        return result;
    }

    /// <summary>
    /// 字符串转换为浮点
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static float toFloat ( this string str ) {
        if (str == null) { return 0; }
        float result = 0;
        float.TryParse (str, out result);
        return result;
    }
    /// <summary>
    /// 字符串转换为整型数组
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static int[] toIntArray ( this string str, char c ) {
        string[] strs = str.Split (c);
        int[] arr = new int[strs.Length];
        for (int i = 0; i<strs.Length; i++) {
            arr[i] = strs[i].toInt ();
        }
        return arr;
    }

    /// <summary>
    /// 字符串转换为長整型数组
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static long[] toLongArray ( this string str, char c ) {
        string[] strs = str.Split (c);
        long[] arr = new long[strs.Length];
        for (int i = 0; i<strs.Length; i++) {
            arr[i] = strs[i].toLong ();
        }
        return arr;
    }
    /// <summary>
    /// 字符串转换为浮点型数数组
    /// </summary>
    /// <param name="str">格式化控制字符串</param>
    /// <returns></returns>
    [System.Diagnostics.DebuggerHidden]
    public static float[] toFloatArray ( this string str, char c ) {
        string[] strs = str.Split (c);
        float[] arr = new float[strs.Length];
        for (int i = 0; i<strs.Length; i++) {
            arr[i] = strs[i].toFloat ();
        }
        return arr;
    }
    #endregion



    #region 路径
    /// <summary>
    /// 通过文件名获取扩展名
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string extension ( this string str ) {
        return Path.GetExtension (str);
    }

    /// <summary>
    /// 获取文件名路径的文件夹路径
    /// </summary>
    public static string dir ( this string path ) {
        return Path.GetDirectoryName (path).Replace ('\\', '/');
    }

    /// <summary>
    /// 路径是否是文件夹
    /// </summary>
    public static bool isdir ( this string path ) {
        if (path != null && path.Length > 0 &&
            (path[path.Length - 1] == '/' || path[path.Length - 1] == '\\'))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 获取文件名路径的文件名
    /// </summary>
    public static string file ( this string path ) {
        string name = Path.GetFileName (path);
        if (!name.valid ())
            name = Path.GetFileName (Path.GetDirectoryName (path));
        return name;
    }

    /// <summary>
    /// 获取沒有扩展的文件名
    /// </summary>
    public static string fileNoExt ( this string path ) {
        return Path.GetFileNameWithoutExtension (path);
    }
    #endregion


    #region MD5
    public static string GetMD5 ( byte[] bytes ) {
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
        byte[] hashBytes = md5.ComputeHash (bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString (hashBytes[i], 16).PadLeft (2, '0');
        }

        return hashString.PadLeft (32, '0');

    }

    /// <summary>
    /// 获取字符串的 MD5
    /// </summary>
    /// <param name="strToEncrypt"></param>
    /// <returns></returns>
    public static string MD5 ( this string strToEncrypt ) {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
        byte[] bytes = ue.GetBytes (strToEncrypt);
        return GetMD5 (bytes);
    }

    /// <summary>
    /// 获取文件的 MD5
    /// </summary>
    /// <param name="path"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string FileMD5 ( this string path, out long size ) {
        //获得文件大小
        FileInfo info = new FileInfo (path);
        size = info.Length;

        if (info.Name.Substring (0, 1) == ".") {
            return null;
        }

        byte[] byData = new byte[size];
        FileStream stream = File.OpenRead (path);
        stream.Read (byData, 0, (int)size);
        stream.Flush ();
        stream.Close ();


        return GetMD5 (byData);
    }
    #endregion

    /// <summary>
    /// 判断字符串是否 不为 null 或者 空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool valid ( this string str ) {
        return !string.IsNullOrEmpty (str);
    }

    /// <summary>
    /// 获取数据中的换行
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public static string lineEnd ( this string lines ) {
        if (lines.IndexOf ("\r\n") != -1)
            return "\r\n";

        if (lines.IndexOf ("\n") != -1)
            return "\n";

        if (lines.IndexOf ("\r") != -1)
            return "\r";

        return null;

    }

    /// <summary>
    /// 获取最后一个字符串
    /// </summary>
    /// <param name="strs"></param>
    /// <returns></returns>
    public static string last ( this string[] strs ) {
        for (int i = strs.Length - 1; i > 0; i--)
            if (strs[i].valid ())
                return strs[i];

        return strs[0];
    }

    /// <summary>
    /// 取 x 模 m 的非负数
    /// </summary>
    public static int modulo ( this int x, int m ) {
        return (x%m + m)%m;
    }


    #region Genic List
    /// <summary>
    /// 判断不为空，并且长度大于0
    /// </summary>
    public static bool valid<T> ( this List<T> list ) {
        return list != null && list.Count > 0;
    }

    /// <summary>
    /// 获取个数
    /// </summary>
    public static int max<T> ( this List<T> list ) {
        return list.Count;
    }

    /// <summary>
    /// 判断不为空，并且长度大于0
    /// </summary>
    public static bool valid<T> ( this T[] array ) {
        return array != null && array.Length > 0;
    }

    /// <summary>
    /// 获取个数
    /// </summary>
    public static int max<T> ( this T[] array ) {
        return array.Length;
    }

    /// <summary>
    /// 找到匹配条目的索引
    /// </summary>
    public static int indexOf<T> ( this T[] array, System.Predicate<T> match ) {
        if (array == null || array.Length == 0 || match == null) { return -1; }

        for (int i = 0; i<array.Length; i++) {
            if (match (array[i]))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 判断不为空，并且长度大于0
    /// </summary>
    public static bool valid ( this ArrayList array ) {
        return array != null && array.Count > 0;
    }

    /// <summary>
    /// 获取个数
    /// </summary>
    public static int max ( this ArrayList array ) {
        return array.Count;
    }

    #endregion

    /// <summary>
    /// 截取字符串到指定长度
    /// </summary>
    public static string sub ( this string value, int length, char padding = '\0' ) {
        if (value.Length > length) { return value.Substring (0, length); }
        else if (value.Length == length) { return value; }
        else if (padding != '\0') { return value.PadLeft (length - value.Length, padding); }
        return value;
    }
}

