using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] bool autoUpdate = true;
    [Range(2, 256)]
    [SerializeField] int resolution = 10;
    [SerializeField] Material material;
    [SerializeField] public PlanetShape shape;
    [SerializeField] public PlanetColour colour;
    [SerializeField] public enum FaceRenderMask {All, Top, Bottom, Left, Right, Front, Back };
    [SerializeField] public FaceRenderMask faceRenderMask;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColourGenerator colourGenerator = new ColourGenerator();

    [HideInInspector] public bool colourFoldout;
    [HideInInspector] public bool shapeFoldout;
    int numFaces = 6;

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
    }
    
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shape);
        colourGenerator.UpdateSettings(colour);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[numFaces];
        }
        terrainFaces = new TerrainFace[numFaces];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < numFaces; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject mesh = new GameObject("mesh");
                mesh.transform.parent = transform;

                mesh.AddComponent<MeshRenderer>();
                meshFilters[i] = mesh.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colour.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderMask = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderMask);
        }
    }
    void GenerateMesh()
    {
        for (int i = 0; i < numFaces; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ContructMesh();
            }
        }
        colourGenerator.UpdateElevation(shapeGenerator.planetMinMax);
    }


    void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (int i = 0; i < numFaces; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
        
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }
}
