using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChecklistBot
{

    public class Rootobject
    {
        public string luis_schema_version { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string culture { get; set; }
        public Intent[] intents { get; set; }
        public object[] entities { get; set; }
        public object[] composites { get; set; }
        public object[] bing_entities { get; set; }
        public object[] actions { get; set; }
        public object[] model_features { get; set; }
        public object[] regex_features { get; set; }
        public Utterance[] utterances { get; set; }
    }

    public class Intent
    {
        public string name { get; set; }
    }

    public class Utterance
    {
        public string text { get; set; }
        public string intent { get; set; }
        public object[] entities { get; set; }
    }

}