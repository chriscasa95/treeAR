using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using Mono.Cecil.Cil;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleGenerator : MonoBehaviour
{
    Database database = new Database();
    Tree tree = null;
    string treeName = "HopeaOdorata_B1";


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


    public Rect windowRect = new Rect(0, 0, 200, 220);

    public bool includeChildren = true;

    void OnGUI()
    {
        windowRect = GUI.Window("ParticleController".GetHashCode(), windowRect, DrawWindowContents, ps.name);
    }

    void DrawWindowContents(int windowId)
    {
        if (ps == true)
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

            GUILayout.BeginHorizontal();
            treeName = GUILayout.TextArea(treeName, 100);
            GUILayout.EndHorizontal();

            updateTree(treeName);
        }
        else
            GUILayout.Label("No Particle System found");

        GUI.DragWindow();
    }

    async void updateTree(string tree_name)
    {
        try
        {
            var lifeTime = 20;
            var leafMultiplier = 10;
            tree = await database.GetTreeAsync(tree_name);

            // pos
            Vector3 position = transform.position;
            position.y = tree.Cone_hight_m;
            gameObject.transform.position = position;

            // main
            var main = ps.main;
            main.maxParticles = tree.Leafe_amount * leafMultiplier;
            main.startSize = tree.Leaf_size_m;
            main.startLifetime = lifeTime;
            main.startSpeed = 1;

            // shape 
            var shape = ps.shape;
            shape.radius = tree.Cone_width_m;

            // emission
            var emission = ps.emission;
            emission.rateOverTime = main.maxParticles / lifeTime;

            // material
            //psr.sharedMaterial = Resources.Load<Material>(tree.Leaf_material_ID);
        }
        catch (Exception e)
        {
            //  Block of code to handle errors
        }
       
    }
}