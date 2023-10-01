using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvKey
{
    // Airtable
    public static string APIVERSION { get { return "v0"; } }
    public static string APPTOKEN { get { return "replace by yout table token here)"; } } // Table token can be found in your Airtable Database url, starting with "app"
    public static string APIKEY { get { return "Replace by your Airtable Api key"; } } // Api key can be found in your Airtable Developer hub


    // Google Drive
    public static string GOOGLE_APIKEY { get { return "Replace by your google drive Api key"; } }
    

    // Airtable Table Names
    public static class Tables
    {
        public static string ConfigSetup { get { return "ConfigSetup"; } }
        public static string UnityConfig { get { return "UnityConfig"; } }
        public static string SurveyVersion { get { return "SurveyVersion"; } }
        public static string UABList { get { return "UABList"; } }
        public static string ResponseVersion { get { return "ResponseVersion"; } }
    }
}
