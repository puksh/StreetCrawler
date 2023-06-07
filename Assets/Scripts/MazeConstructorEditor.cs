using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeConstructor))]
public class MazeConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Display a button to generate the maze in the Editor
        if (GUILayout.Button("Generate Maze"))
        {
            MazeConstructor mazeConstructor = (MazeConstructor)target;
            mazeConstructor.GenerateMazeInEditor(27, 29); // Adjust the sizeRows and sizeCols as desired
        }
    }

}