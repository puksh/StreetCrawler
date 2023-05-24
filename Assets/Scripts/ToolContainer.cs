using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    public Transform tool1;
    public Transform tool2;
    public int currentToolIndex = 1;

    private void Start()
    {
        // Disable the second tool's renderer
        tool1.GetComponent<Renderer>().enabled = false;
    }

    public void SwitchTool()
    {
        // Disable the current tool's renderer
        if (currentToolIndex == 0)
        {
            tool1.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            tool2.GetComponent<Renderer>().enabled = false;
        }

        // Enable the new tool's renderer
        if (currentToolIndex == 0)
        {
            tool2.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            tool1.GetComponent<Renderer>().enabled = true;
        }

        Debug.Log("Current tool index: " + currentToolIndex);
        Debug.Log("Tool 1 is visible: " + tool1.GetComponent<Renderer>().enabled);
        Debug.Log("Tool 2 is visible: " + tool2.GetComponent<Renderer>().enabled);
        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void Update()
    {
        // Check if the player scrolls the mouse wheel up
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            // Increase the current tool index by 1
            currentToolIndex++;
            if (currentToolIndex > 1)
            {
                currentToolIndex = 0;
            }
            SwitchTool();
        }
        // Check if the player scrolls the mouse wheel down
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            // Decrease the current tool index by 1
            currentToolIndex--;
            if (currentToolIndex < 0)
            {
                currentToolIndex = 1;
            }
            SwitchTool();
        }
    }
}

