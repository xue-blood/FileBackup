using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackup
{
    /// <summary>
    /// path process interface
    /// order @isexclude -> @isinclude -> @remove -> @rename
    /// </summary>
    public interface IExProcess
    {
        bool isexclude(string path, List<string> patten);
        bool isinclude(string path, List<string> patten);
        string remove(string path, List<string> patten);
        string rename(string path, List<string> patten);
        string outdir(string path, int passcount);
        void init_split(string split, int total, string format);
        void init(string path, string root);
    }

    public interface ISubProcess
    {
        string key { get; }
        void init(string path, string root);
        bool haspatten(string path, string patten);
        bool remove(StringBuilder path, string patten);
        bool rename(StringBuilder path, string patten);
    }
    class ExProcess : IExProcess
    {
        PlainProcess plain = new PlainProcess();
        InteProcess inte = new InteProcess();

        public void init(string path, string root) {
            inte.init(path, root);
            plain.init(path, root);
        }

        public bool isexclude(string path, List<string> patten) {
            if (patten == null) return false;
            for (int i = 0; i < patten.Count; i++) {
                if (inte.haspatten(path, patten[i])) return true;
                if (plain.haspatten(path, patten[i])) return true;
            }
            return false;
        }

        public bool isinclude(string path, List<string> patten) {
            if (patten == null) return false;
            for (int i = 0; i < patten.Count; i++) {
                if (inte.haspatten(path, patten[i])) return true;
                if (plain.haspatten(path, patten[i])) return true;
            }
            return false;

        }
        int totalcount = 0;
        int splitcount = 0;
        string splitformat = "_{p}";
        string splitformat_count = "{c}", splitformat_count1 = "{count}"; // 当前个数
        string splitformat_piece = "{p}", splitformat_piece1 = "{piece}"; // 当前块数
        string splitformat_piece_s = "{s}", splitformat_piece_start = "{start}"; // 当前块开始数
        string splitformat_piece_e = "{e}", splitformat_piece_end = "{end}";     // 当前块结束数
        string splitformat_piece_t = "{t}", splitformat_piece_total = "{total}"; // 总数
        public void init_split(string split, int total, string format)
        {
            if (format.valid())
                splitformat = format;
            if (!split.valid())
            {
                splitcount = 0;
                return;
            }
            var ss = split.Split('/'); // 1 / 5,  or 5
            if (ss.Length == 1)
                splitcount = int.Parse(ss[0]);
            else
                splitcount = total * int.Parse(ss[0]) / int.Parse(ss[1]);
            if(splitcount == total)
                splitcount = 0;
            totalcount = total;
        }

        public string outdir(string path, int passcount)
        {
            if (splitcount <= 0) return path;
            StringBuilder sbd = new StringBuilder(splitformat);

            sbd.Replace(splitformat_count, passcount.ToString());
            sbd.Replace(splitformat_count1, passcount.ToString());

            int piece = passcount / splitcount;

            sbd.Replace(splitformat_piece, piece.ToString());
            sbd.Replace(splitformat_piece1, piece.ToString());

            sbd.Replace(splitformat_piece_s, (piece * splitcount).ToString());
            sbd.Replace(splitformat_piece_start, (piece * splitcount).ToString());

            sbd.Replace(splitformat_piece_e, (piece * splitcount + splitcount - 1).ToString());
            sbd.Replace(splitformat_piece_end, (piece * splitcount + splitcount - 1).ToString());

            sbd.Replace(splitformat_piece_t, totalcount.ToString());
            sbd.Replace(splitformat_piece_total, totalcount.ToString());

            //return path + "_" + piece;
            return path + sbd.ToString();
        }

        public string remove(string path, List<string> patten) {
            if (patten == null) return path;
            var sbd = new StringBuilder(path);
            for (int i = 0; i < patten.Count; i++) { 
                if (inte.remove(sbd, patten[i])) continue;
                if (plain.remove(sbd, patten[i])) continue;
            }
            return sbd.ToString();
        }

        public string rename(string path, List<string> patten) {
            if (patten == null) return path;
            var sbd = new StringBuilder(path);
            for (int i = 0; i < patten.Count; i++) { 
                if (inte.rename(sbd, patten[i])) continue;
                if (plain.rename(sbd, patten[i])) continue;
            }
            return sbd.ToString();
        }
    }

    class PlainProcess : ISubProcess
    {
        public string key => "";

        public bool haspatten(string path, string patten) {
            return path.Contains(patten);
        }

        public void init(string path, string root) {
            
        }

        public bool remove(StringBuilder path, string patten) {
            path.Replace(patten, "");
            return true;
        }

        public bool rename(StringBuilder path, string patten) {
            if (patten.Length == 2)
                path.Replace(patten[0], patten[1]); // just one character
            else
                path.Replace(patten.Split(':')[0], patten.Split(':')[1]); // string replace
            return true;
        }
    }

    class InteProcess : ISubProcess
    {
        public string key => throw new NotImplementedException();

        const string key_depth = "depth";   // path depth with relative path
        const string key_file = "file";     // file name with ext
        const string key_name = "name";     // file name without ext
        const string key_ext = "ext";       // the ext of file
        int depth = 0;
        string[] dir;                       // todo dir compare
        string file;
        string name;
        string ext;
        public bool haspatten(string path, string patten)
        {
            if (patten.StartsWith(key_depth))
                return depth.math(patten.Remove(0, key_depth.Length));
            else if (patten.StartsWith(key_file))
                return file.math(patten.Remove(0, key_file.Length));
            else if (patten.StartsWith(key_name))
                return name.math(patten.Remove(0, key_name.Length));
            else if (patten.StartsWith(key_ext))
                return ext.math(patten.Remove(0, key_ext.Length));
            return false;
        }

        public bool remove(StringBuilder path, string patten)
        {
            return false;
        }

        public bool rename(StringBuilder path, string patten)
        {
            return false;
        }

        public void init(string path, string root) {
            depth = path.Replace(root,"").Split('/', '\\').Length - 1;
            name = System.IO.Path.GetFileNameWithoutExtension(path);
            file = System.IO.Path.GetFileName(path);
            ext = System.IO.Path.GetExtension(path);
        }
    }
}