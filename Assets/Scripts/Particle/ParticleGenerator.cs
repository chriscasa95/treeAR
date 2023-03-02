using UnityEngine;
using UnityEditor;
using System;
using TMPro;
using Newtonsoft.Json;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Unity.VisualScripting;
using static UnityEngine.AudioSettings;

public class ParticleGenerator : MonoBehaviour
{

    [SerializeField] private bool debug_on;
    
    Database database = new Database();
    Tree tree = null;
    string treeName = "HopeaOdorata_B1";
    string logtxt = "";

    void Start()
    {
    }

    ParticleSystem ps
    {
        get
        {
            if (_CachedSystem == null)
                _CachedSystem = GetComponent<ParticleSystem>();
            return _CachedSystem;
        }
    }
    private ParticleSystem _CachedSystem;

    ParticleSystemRenderer psr
    {
        get
        {
            if (_CachedRenderer == null)
                _CachedRenderer = GetComponent<ParticleSystemRenderer>();
            return _CachedRenderer;
        }
    }
    private ParticleSystemRenderer _CachedRenderer;


    public Rect windowRect = new Rect(0, 0, 400, 20);

    public bool includeChildren = true;

    public bool showElements = true;

    private void Update()
    {
        if (debug_on == false)
        {
            GameObject sign = GameObject.Find("sign_plant_SEEE");
            GameObject text_plane = GameObject.Find("TextPlane");

            print(CloudRecoHandler.mTargetFound);

            if (CloudRecoHandler.mTargetFound == true)
            {
                print("Target active");
                changeVisibility(sign, true);
                changeVisibility(text_plane, true);
                ps.Play(includeChildren);


                FullMetaData meta = JsonConvert.DeserializeObject<FullMetaData>(CloudRecoHandler.mTargetMetadata);
                updateTreeMetadata(meta);

                //GameObject plane = GameObject.Find("Ground Plane Stage");
                //GameObject image = GameObject.Find("ImageTarget");
                //Vector3 vec = image.transform.position;

                //vec.y = plane.transform.position.y;
                //plane.transform.position = vec;
            }
            else
            {
                print("Target not active");
                ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);

                changeVisibility(sign, false);
                changeVisibility(text_plane, false);
            }
        }

    } 



    void OnGUI()
    {
        windowRect = GUI.Window("ParticleController".GetHashCode(), windowRect, DrawWindowContents, ps.name);
    }

    void DrawWindowContents(int windowId)
    {


        if (debug_on == true)
        {

            GUILayout.BeginHorizontal();
            GUILayout.Toggle(ps.isPlaying, "Playing");
            GUILayout.Toggle(ps.isEmitting, "Emitting");
            GUILayout.Toggle(ps.isPaused, "Paused");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
                ps.Play(includeChildren);
            if (GUILayout.Button("Pause"))
                ps.Pause(includeChildren);
            if (GUILayout.Button("Stop Emitting"))
                ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
            if (GUILayout.Button("Stop & Clear"))
                ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            includeChildren = GUILayout.Toggle(includeChildren, "Include Children");
            showElements = GUILayout.Toggle(showElements, "Show Elements");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Time(" + ps.time + ")");
            GUILayout.Label("Particle Count(" + ps.particleCount + ")");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            treeName = GUILayout.TextArea(treeName, 100);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Found: " + CloudRecoHandler.mTargetFound);
            GUILayout.EndHorizontal();

            GUILayout.Label(logtxt, GUILayout.Height(500));

            updateTree(treeName);

            GameObject sign = GameObject.Find("sign_plant_SEEE");
            GameObject text_plane = GameObject.Find("TextPlane");

            if (showElements)
            {
                changeVisibility(sign, true);
                changeVisibility(text_plane, true);
            } else
            {
                changeVisibility(sign, false);
                changeVisibility(text_plane, false);
            }

        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Toggle(ps.isPlaying, "Playing");
            GUILayout.Toggle(ps.isEmitting, "Emitting");
            GUILayout.Toggle(ps.isPaused, "Paused");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Time(" + ps.time + ")");
            GUILayout.Label("Particle Count(" + ps.particleCount + ")");
            GUILayout.EndHorizontal();

            GUILayout.Label("Target active: " + CloudRecoHandler.mTargetFound);

            GameObject plane = GameObject.Find("Ground Plane Stage");
            Vector3 vec = plane.transform.position;
            GUILayout.Label("Tree x:" + vec.x + " y:" + vec.y + " z:" + vec.z);

            try
            {
                FullMetaData meta = JsonConvert.DeserializeObject<FullMetaData>(CloudRecoHandler.mTargetMetadata);
                GUILayout.Label(meta.Plant_name_scientific);

            } catch(Exception e)
            {

            }

        }


        GUI.DragWindow();
    }

    async void updateTree(string tree_name)
    {
        try
        {
            GameObject headline = GameObject.Find("headline");
            GameObject plant_names = GameObject.Find("plant_names_txt");
            GameObject plant_family = GameObject.Find("plant_family_txt");
            GameObject occurence = GameObject.Find("occurence_txt");
            GameObject growing_conditions = GameObject.Find("growing_conditions_txt");
            GameObject info = GameObject.Find("info_txt");
            var headline_txt = headline.GetComponent<TextMeshPro>();
            var plant_names_txt = plant_names.GetComponent<TextMeshPro>();
            var plant_family_txt = plant_family.GetComponent<TextMeshPro>();
            var occurence_txt = occurence.GetComponent<TextMeshPro>();
            var growing_conditions_txt = growing_conditions.GetComponent<TextMeshPro>();
            var info_txt = info.GetComponent<TextMeshPro>();



            var lifeTime = 10;
            var leafMultiplier = 10;
            tree = await database.GetTreeAsync(tree_name);

            // pos
            Vector3 position = transform.position;
            position.y = tree.Cone_hight_m;
            gameObject.transform.position = position;

            // main
            var main = ps.main;
            main.maxParticles = 20000;
            //main.maxParticles = tree.Leafe_amount * leafMultiplier;
            main.startLifetime = lifeTime;
            main.startSpeed = 1;

            // leaf size
            var constantMin = tree.Leaf_size_m - tree.Leaf_size_m * 0.2f;
            var constantMax = tree.Leaf_size_m + tree.Leaf_size_m * 0.2f;
            main.startSize = new ParticleSystem.MinMaxCurve(constantMin, constantMax);

            main.gravityModifier = 0.5f;

            // if palmtree :)
            if (constantMin >= 1) { 
                main.gravityModifier = 1.5f;
            }

            // shape 
            var shape = ps.shape;
            shape.radius = tree.Cone_width_m;

            // emission
            var emission = ps.emission;
            emission.rateOverTime = tree.Leafe_amount * leafMultiplier / lifeTime;

            // material
            psr.sharedMaterial = Resources.Load<Material>(tree.Leaf_material_ID);

            // text
            headline_txt.text = tree.Plant_name_scientific;
            plant_names_txt.text = tree.Plant_name_EN;
            plant_family_txt.text = tree.Plant_family;
            occurence_txt.text = tree.Occurrence;
            growing_conditions_txt.text = tree.Growing_conditions;
            info_txt.text = tree.Plant_description_long_EN;

        }
        catch (Exception e)
        {
            print(e);
            logtxt = e.ToString();
        }
       
    }


    async void updateTreeMetadata(FullMetaData tree)
    {
        try
        {
            GameObject headline = GameObject.Find("headline");
            GameObject plant_names = GameObject.Find("plant_names_txt");
            GameObject plant_family = GameObject.Find("plant_family_txt");
            GameObject occurence = GameObject.Find("occurence_txt");
            GameObject growing_conditions = GameObject.Find("growing_conditions_txt");
            GameObject info = GameObject.Find("info_txt");
            var headline_txt = headline.GetComponent<TextMeshPro>();
            var plant_names_txt = plant_names.GetComponent<TextMeshPro>();
            var plant_family_txt = plant_family.GetComponent<TextMeshPro>();
            var occurence_txt = occurence.GetComponent<TextMeshPro>();
            var growing_conditions_txt = growing_conditions.GetComponent<TextMeshPro>();
            var info_txt = info.GetComponent<TextMeshPro>();



            var lifeTime = 10;
            var leafMultiplier = 30;

            // pos
            Vector3 position = transform.position;
            position.y = tree.Cone_hight_m;
            gameObject.transform.position = position;

            // main
            var main = ps.main;
            main.maxParticles = 20000;
            //main.maxParticles = tree.Leafe_amount * leafMultiplier;
            main.startLifetime = lifeTime;
            main.startSpeed = 1;

            // leaf size
            var constantMin = tree.Leaf_size_m - tree.Leaf_size_m * 0.2f;
            var constantMax = tree.Leaf_size_m + tree.Leaf_size_m * 0.2f;
            main.startSize = new ParticleSystem.MinMaxCurve(constantMin, constantMax);

            main.gravityModifier = 0.5f;

            // if palmtree :)
            if (constantMin >= 1)
            {
                main.gravityModifier = 1.5f;
            }

            // shape 
            var shape = ps.shape;
            shape.radius = tree.Cone_width_m;

            // emission
            var emission = ps.emission;
            emission.rateOverTime = tree.Leafe_amount * leafMultiplier / lifeTime;

            // material
            psr.sharedMaterial = Resources.Load<Material>(tree.Leaf_material_ID);

            // text
            headline_txt.text = tree.Plant_name_scientific;
            plant_names_txt.text = tree.Plant_name_EN;
            plant_family_txt.text = tree.Plant_family;
            occurence_txt.text = tree.Occurrence;
            growing_conditions_txt.text = tree.Growing_conditions;
            info_txt.text = tree.Plant_description_long_EN;

        }
        catch (Exception e)
        {
            print(e);
            logtxt = e.ToString();
        }

    }

    public void changeVisibility(GameObject game, bool visible)
    {
        Debug.Log(game.transform.childCount);
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[game.transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in game.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now change render state
        foreach (GameObject child in allChildren)
        {
            try
            {
                child.GetComponent<Renderer>().enabled = visible;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}

public class MetaData
{
    public string Plant_ID { get; set; }
}

public class FullMetaData
{
    public string Plant_ID { get; set; }
    public string Plant_name_EN { get; set; }
    public string Plant_name_scientific { get; set; }
    public string Plant_name_VN { get; set; }
    public float Plant_size_m { get; set; }
    public string Leaf_material_ID { get; set; }
    public float Leaf_size_m { get; set; }
    public float Leafe_amount { get; set; }
    public float Cone_hight_m { get; set; }
    public float Cone_width_m { get; set; }
    public float LeaveFall_width_m { get; set; }
    //public float Sign_position { get; set; }
    //public float Sign_angle { get; set; }
    //public float Sign_size { get; set; }
    public string Plant_family { get; set; }
    public string Occurrence { get; set; }
    public string Growing_conditions { get; set; }
    public string Plant_description_long_EN { get; set; }
    public string Plant_description_long_VN { get; set; }
    public string Plant_description_long_DE { get; set; }
    //public float Text_size { get; set; }


}
