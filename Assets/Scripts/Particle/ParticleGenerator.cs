using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using Mono.Cecil.Cil;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleGenerator : MonoBehaviour
{
    Tree tree = null;
    
    string pos0 = "0";
    string pos1 = "0";
    string pos2 = "0";
    string particleAmount = "100";
    string emissionRate = "20";
    string particleSize = "1";
    string startSpeed = "1";
    string startLifeTime = "10";
    string coneRadius = "1";

    void Start()
    {
        Vector3 position = transform.position;
        pos0 = string.Format("{0:N3}", position.x);
        pos1 = string.Format("{0:N3}", position.y);
        pos2 = string.Format("{0:N3}", position.z);
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


    public Rect windowRect = new Rect(0, 0, 400, 220);

    public bool includeChildren = true;

    void OnGUI()
    {
        windowRect = GUI.Window("ParticleController".GetHashCode(), windowRect, DrawWindowContents, ps.name);
    }

    void DrawWindowContents(int windowId)
    {
        if (ps)
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

            includeChildren = GUILayout.Toggle(includeChildren, "Include Children");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Time(" + ps.time + ")");
            GUILayout.Label("Particle Count(" + ps.particleCount + ")");
            GUILayout.EndHorizontal();
            
            // Position
            GUILayout.BeginHorizontal();
            GUILayout.Label("Pos_x:");
            pos0 = GUILayout.TextArea(pos0, 10);
            
            GUILayout.Label("Pos_z:");
            pos1 = GUILayout.TextArea(pos1, 10);
            
            GUILayout.Label("Pos_y:");
            pos2 = GUILayout.TextArea(pos2, 10);
            GUILayout.EndHorizontal();
            

            // Particle system
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max Amount:");
            particleAmount = GUILayout.TextArea(particleAmount, 10);

            GUILayout.Label("Emission R:");
            emissionRate = GUILayout.TextArea(emissionRate, 10);

            GUILayout.Label("LifeTime:");
            startLifeTime = GUILayout.TextArea(startLifeTime, 10);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("P. Size:");
            particleSize = GUILayout.TextArea(particleSize, 10);

            GUILayout.Label("Start Speed:");
            startSpeed = GUILayout.TextArea(startSpeed, 10);

            GUILayout.Label("Cone R:");
            coneRadius = GUILayout.TextArea(coneRadius, 10);
            GUILayout.EndHorizontal();


            // position
            gameObject.transform.position = new Vector3(float.Parse(pos0), float.Parse(pos1), float.Parse(pos2));

            // main
            var main = ps.main;
            main.maxParticles = int.Parse(particleAmount);
            main.startSize = float.Parse(particleSize);
            main.startLifetime = float.Parse(startLifeTime);
            main.startSpeed = float.Parse(startSpeed);

            // shape 
            var shape = ps.shape;
            shape.radius = float.Parse(coneRadius);

            // emission
            var emission = ps.emission;
            emission.rateOverTime = float.Parse(emissionRate);

            // material
            psr.sharedMaterial = Resources.Load<Material>("NewLeaves");

        }
        else
            GUILayout.Label("No Particle System found");

        GUI.DragWindow();
    }

    async void updateTree(string tree_name)
    {

        Database database = new Database();
        tree = await database.GetTreeAsync(tree_name);

        // pos
        gameObject.transform.position = new Vector3(0, 0, tree.Cone_hight_m);
       
        // main
        var main = ps.main;
        main.maxParticles = tree.Leafe_amount;
        main.startSize = tree.Leaf_size_m;
        main.startLifetime = 100;
        main.startSpeed = 1;

        // shape 
        var shape = ps.shape;
        shape.radius = tree.Cone_width_m;

        // emission
        var emission = ps.emission;
        emission.rateOverTime = main.maxParticles/100;

        // material
        //psr.sharedMaterial = Resources.Load<Material>(tree.Leaf_material_ID);
    }
}