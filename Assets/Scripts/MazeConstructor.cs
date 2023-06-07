using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    public int[,] data { get; private set; }

    void Awake()
    {
        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler startCallback = null)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }

        DisposeOldMaze();

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        FindStartPosition();
        FindGoalPosition();

        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceMultipleGoalTriggers(10);
    }

    private void DisplayMaze()
    {
        GameObject go = GameObject.Find("Procedural Maze");
        if (go == null)
        {
            go = new GameObject("Procedural Maze");
            go.tag = "Generated";
        }
        go.transform.position = Vector3.zero;

        MeshFilter mf = go.GetComponent<MeshFilter>();
        if (mf == null)
            mf = go.AddComponent<MeshFilter>();

        mf.sharedMesh = meshGenerator.FromData(data);

        MeshCollider mc = go.GetComponent<MeshCollider>();
        if (mc == null)
            mc = go.AddComponent<MeshCollider>();

        mc.sharedMesh = mf.sharedMesh;

        MeshRenderer mr = go.GetComponent<MeshRenderer>();
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

    /*void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }

        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }*/

    public float hallWidth { get; private set; }
    public float hallHeight { get; private set; }

    public int startRow { get; private set; }
    public int startCol { get; private set; }

    public int goalRow { get; private set; }
    public int goalCol { get; private set; }

    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            if (go.name == "Procedural Maze")
                DestroyImmediate(go);
            else
                DestroyImmediate(go);
        }
    }

    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }

    private void FindGoalPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
                    return;
                }
            }
        }
    }

    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.Find("Start Trigger");
        if (go == null)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Start Trigger";
            go.tag = "Generated";
        }

        go.transform.position = new Vector3(startCol * hallWidth, 0.5f, startRow * hallWidth);

        go.GetComponent<BoxCollider>().isTrigger = true;

        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = go.AddComponent<MeshRenderer>();

        mr.sharedMaterial = startMat;

        TriggerEventRouter tc = go.GetComponent<TriggerEventRouter>();
        if (tc == null)
            tc = go.AddComponent<TriggerEventRouter>();

        tc.callback = callback;
    }

    private void PlaceMultipleGoalTriggers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Find("Treasure " + i);
            if (go == null)
            {
                go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Materials/BugGrass.prefab");
                go = Instantiate(go);
                go.name = "Treasure " + i;
                go.tag = "Generated";
                //Debug.Log("Treasure"+1);
            }

            go.transform.position = new Vector3(goalCol * hallWidth, 2f, goalRow * hallWidth);
            
        }
    }
}
