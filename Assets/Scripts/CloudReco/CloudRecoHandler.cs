using UnityEngine;
using Vuforia;
//using static UnityEngine.CullingGroup;
//using static Vuforia.CloudRecoBehaviour;

public class CloudRecoHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;

    private bool mTargetFound = false;
    
    // static makes var a member of class, not instance
    public static string mTargetMetadata = "";
    public static string mTargetName = "";
    public static string mTargetId = "";

    public ImageTargetBehaviour ImageTargetTemplate;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }


    // Methods
    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        Debug.Log("scanning: " + scanning);

        if (scanning)
        {
            // Clear all known targets
            
            //mTargetFound = false;
            //mTargetMetadata = "";
            //mTargetName = "";
            //mTargetId = "";
        }
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetFound    = true;
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        mTargetName = cloudRecoSearchResult.TargetName;
        mTargetId = cloudRecoSearchResult.UniqueTargetId;

        Debug.Log("mTargetName: " + mTargetName + " mTargetId: " + mTargetId);

        // Stop the scanning by disabling the behaviour
        //mCloudRecoBehaviour.enabled = false;

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            /* Enable the new result with the same ImageTargetBehaviour: */
            mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
        }
    }

    void OnGUI()
    {
        //// Display current 'scanning' status
        //GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        //// Display metadata of latest detected cloud-target
        //GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);


        //if (mTargetFound)
        //{
        //    GUI.Box(new Rect(100, 400, 200, 50), "Name: " + mTargetName);
        //}


        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                // Reset Behaviour
                mCloudRecoBehaviour.enabled = true;
                mTargetMetadata = "";
            }
        }
    }
}