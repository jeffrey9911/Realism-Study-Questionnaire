using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvKey
{
    // Airtable
    public static string APIVERSION { get { return "v0"; } }
    public static string APPTOKEN { get { return "apptpwKwOEJtQypv7"; } }
    public static string APIKEY { get { return "keyBWmSRLypBsJUvo"; } }


    // Google Drive
    public static string GOOGLE_APIKEY { get { return "AIzaSyDdfTzQOOQ_Vr4yHTWFliSpVTm3ZRC03PE"; } } 


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
