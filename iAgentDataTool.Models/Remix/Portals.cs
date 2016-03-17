using System;
using System.Text;

namespace iAgentDataTool.Models.Remix
{
    public class Portals
    {
        public class Builder {
            int _id;
            string _url;
            string _description;
            bool _isEnabled;

            public Builder WithId(int value) { _id = value; return this; }
            public Builder WithUrl(string value) { _url = value; return this; }
            public Builder WithDescription(string value) { _description = value; return this; }
            public Builder WithIsEnabled(bool value) { _isEnabled = value; return this; }

            public int Id { get { return _id; } }
            public string Url { get { return _url; } }
            public string Description { get { return _description; } }
            public bool IsEnabled { get { return _isEnabled; } }

            public Portals Build(){ return new Portals(_id, _url, _description, _isEnabled); }
        }

        readonly int _id;
        readonly string _url;
        readonly string _description;
        readonly bool _isEnabled;

        public int Id { get { return _id; } }
        public string Url { get { return _url; } }
        public string Description { get { return _description; } }
        public bool IsEnabled { get { return _isEnabled; } }

        Portals(int id,
                string url ="not found",
                string description="not found",
                bool isEnabled=false) {
                    _id = id;
                    _url = url;
                    _description = description;
                    _isEnabled = isEnabled;
        }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("{0}", this.Id),
                string.Format("{0}", this.Url),
                string.Format("{0}", this.Description),
                string.Format("{0}", this.IsEnabled)                
            });
        }

        public static Builder Build() { return new Builder();}

    }

}
