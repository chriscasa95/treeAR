using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;

public class DrawGUI : MonoBehaviour
{

    // Update is called once per frame
    void OnGUI()
    {
        //CloudRecoHandler.mTargetId;
        //CloudRecoHandler.mTargetMetadata;
        //CloudRecoHandler.mTargetName;


        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 25;

        //Use Main Camera and get position current object, but point position is pivot point
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //Debug.Log("screenPos.z: " + screenPos.z);

        if (this.enabled)
        {
            MetaData deserializedProduct = JsonConvert.DeserializeObject<MetaData>(CloudRecoHandler.mTargetMetadata);

            //And size box, for example 100x50
            GUI.Box(new Rect(screenPos.x + 100, Screen.height + 50 - screenPos.y, 400, 100), "Name:\n" + deserializedProduct.name, myButtonStyle);
            GUI.Box(new Rect(screenPos.x + 100, Screen.height + 200 - screenPos.y, 400, 200), "Metadata:\n" + deserializedProduct.meta, myButtonStyle);
        }


        //GUI.Box(new Rect(100, 400, 200, 50), "Name: " + CloudRecoHandler.mTargetName);
    }
}

public class MetaData
{
    public string name { get; set; }
    public string meta { get; set; }
}