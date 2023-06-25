using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    private List<Vector3> occupiedPositions = new List<Vector3>();

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    public int[,] data { get; private set; }

    private void Awake()
    {
        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0) Debug.LogError("Odd numbers work better for dungeon size.");

        DisposeOldMaze();

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        //FindStartPosition();
        //FindGoalPosition();

        DisplayMaze();

        //PlaceStartTrigger(startCallback);
        //PlaceMultipleGoalTriggers(10, sizeRows, sizeCols);
    }

    private void DisplayMaze()
    {
        var go = GameObject.Find("Procedural Maze");
        if (go == null)
        {
            go = new GameObject("Procedural Maze");
            go.tag = "Generated";
        }

        go.transform.position = Vector3.zero;

        var mf = go.GetComponent<MeshFilter>();
        if (mf == null)
            mf = go.AddComponent<MeshFilter>();

        mf.sharedMesh = meshGenerator.FromData(data);

        var mc = go.GetComponent<MeshCollider>();
        if (mc == null)
            mc = go.AddComponent<MeshCollider>();

        mc.sharedMesh = mf.sharedMesh;

        var mr = go.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = go.AddComponent<MeshRenderer>();

        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }

#if UNITY_EDITOR
    // This method will be called by the custom editor script
    public void GenerateMazeInEditor(int sizeRows, int sizeCols)
    {
        // Generate the maze
        GenerateNewMaze(sizeRows, sizeCols);
    }
#endif

    private bool IsObjectBelowGenerated(int row, int col)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(col * hallWidth, 10f, row * hallWidth), -Vector3.up, out hit))
            return hit.collider.CompareTag("Generated");
        return false;
    }

    public float hallWidth { get; private set; }
    public float hallHeight { get; private set; }

    public int startRow { get; private set; }
    public int startCol { get; private set; }

    public int goalRow { get; private set; }
    public int goalCol { get; private set; }

    public void DisposeOldMaze()
    {
        var objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (var go in objects)
            if (go.name == "Procedural Maze")
                DestroyImmediate(go);
            else
                DestroyImmediate(go);
    }

    private void FindStartPosition()
    {
        var maze = data;
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        for (var i = 0; i <= rMax; i++)
        for (var j = 0; j <= cMax; j++)
            if (maze[i, j] == 0)
            {
                startRow = i;
                startCol = j;
                return;
            }
    }

    private void FindGoalPosition()
    {
        var maze = data;
        var rMax = maze.GetUpperBound(0);
        var cMax = maze.GetUpperBound(1);

        for (var i = rMax; i >= 0; i--)
        for (var j = cMax; j >= 0; j--)
            if (maze[i, j] == 0)
            {
                goalRow = i;
                goalCol = j;
                return;
            }
    }

    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        var go = GameObject.Find("Start Trigger");
        if (go == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Start Trigger";
            go.tag = "Generated";
            go.layer = 6; //Ground
        }

        go.transform.position = new Vector3(startCol * hallWidth, 0.5f, startRow * hallWidth);

        go.GetComponent<BoxCollider>().isTrigger = true;

        var mr = go.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = go.AddComponent<MeshRenderer>();

        mr.sharedMaterial = startMat;

        var tc = go.GetComponent<TriggerEventRouter>();
        if (tc == null)
            tc = go.AddComponent<TriggerEventRouter>();

        tc.callback = callback;
    }


   
}