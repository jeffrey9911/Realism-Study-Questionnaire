using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UseITableExample : MonoBehaviour
{
    // Create an IAirtable object
    IAirtable myAirtable;

    IAirtable assetAirtable;

    bool isPrinted = false;

    private void Start()
    {
        // You must instantiate an IAirtable object before using it
        myAirtable = this.AddComponent<IAirtable>();

        // Initializing IAirtable with APIs would also create a local copy of that table (if TRUE)
        myAirtable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, "QuestionList0", true);

        // Initialize another IAirtable object for getting assets
        assetAirtable = this.AddComponent<IAirtable>();
        // Initializing IAirtable with APIs without creating a local copy of that table (if FALSE)
        assetAirtable.Initialize(EnvKey.APIVERSION, EnvKey.APPTOKEN, EnvKey.APIKEY, EnvKey.Tables.UABList, false);
    }

    private void Update()
    {
        // Check if the local copy of the table is available (if it is downloaded)
        if(myAirtable.isLocalTableAvailable && !isPrinted)
        {
            // use GetField to get the value of a field from a record
            Debug.Log(myAirtable.LocalTable.GetField("0", "ResponseType"));

            // Or use row name and column name to get the value of a field from a record
            Debug.Log(myAirtable.LocalTable.itable["0"]["Asset(0)"]);

            Debug.Log(myAirtable.LocalTable.itable["0"]["ResponseConfig"]);

            // Use record id to get certain asset from the internet
            string assetRecordId = myAirtable.LocalTable.itable["0"]["Asset(0)"];
            assetRecordId = assetRecordId.Replace("[", "").Replace("]", "");
            assetAirtable.GetAsset(assetRecordId, OnResponseGetUAB);

            isPrinted = true;
        }
    }

    // All "Get" functions are asynchronous, so you need to use callback functions to get the result
    private void OnResponseGetUAB(GameObject gameObject, string fileId)
    {
        if (gameObject != null)
        {
            GameObject instObj = Instantiate(gameObject);
            instObj.transform.position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            instObj.name = fileId;
        }
    }
}
