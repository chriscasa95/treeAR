using System.Diagnostics;
using UnityEngine;
using Vuforia;
//using static UnityEngine.CullingGroup;
//using static Vuforia.CloudRecoBehaviour;

public class CloudRecoHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;

    // static makes var a member of class, not instance
    public static bool mIsScanning = false;
    public static bool mTargetFound = false;
    public static string mTargetMetadata = "";
    public static string mTargetName = "";
    public static string mTargetId = "";

    public ImageTargetBehaviour ImageTargetTemplate;

    Stopwatch sw = new Stopwatch();

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
        print("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        print("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        print("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            //Clear all known targets
            if (sw.ElapsedMilliseconds > 10000)
            {
                mTargetFound = false;
                sw.Stop();
                sw.Reset();
            }
        }

        print("scanning: " + scanning);
        print("Target found: " + mTargetFound);
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetFound = true;
        sw.Start();

        mTargetMetadata = cloudRecoSearchResult.MetaData;
        mTargetName = cloudRecoSearchResult.TargetName;
        mTargetId = cloudRecoSearchResult.UniqueTargetId;

        print("mTargetName: " + mTargetName + " mTargetId: " + mTargetId);

        GameObject plane = GameObject.Find("Ground Plane Stage");
        GameObject image = GameObject.Find("ImageTarget");
        Vector3 vec = image.transform.position;

        vec.y = plane.transform.position.y;
        plane.transform.position = vec;


        // Stop the scanning by disabling the behaviour
        //mCloudRecoBehaviour.enabled = false;

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            /* Enable the new result with the same ImageTargetBehaviour: */
            mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
        }
    }

}