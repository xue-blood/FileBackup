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