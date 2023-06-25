using UnityEngine;
using UnityEngine.InputSystem;


public class ToolSwitcher : MonoBehaviour
{
    public Transform tool1;
    public Transform tool2;
    public int currentToolIndex;


    string child1Name = "Lid";
    string child2Name = "JarLight";
    string flashlightLight = "WhiteLight";

    private Renderer glassLid;
    private Light jarLight;
    private Light flashlight;

    private void Awake()
    {
        glassLid = tool1.Find(child1Name).GetComponent<Renderer>();
        jarLight = tool1.Find(child2Name).GetComponent<Light>();
        flashlight = tool2.Find(flashlightLight).GetComponent<Light>();

        tool1.GetComponent<Renderer>().enabled = false;
        glassLid.enabled = false;
        jarLight.enabled = false;

        tool2.GetComponent<Renderer>().enabled = true;
        flashlight.enabled = true;

    }

    public void SwitchTool()
    {
        // Disable the current tool's renderer
        if (currentToolIndex == 0)
        {
            tool1.GetComponent<Renderer>().enabled = false;
            glassLid.enabled = false;
            jarLight.enabled = false;

            tool2.GetComponent<Renderer>().enabled = true;
            //flashlight.enabled = true;
        }

        // Enable the new tool's renderer
        if (currentToolIndex == 1)
        {
            tool1.GetComponent<Renderer>().enabled = true;
            glassLid.enabled = true;
            jarLight.enabled = true;

            tool2.GetComponent<Renderer>().enabled = false;
            flashlight.enabled = false;
        }

        //Debug.Log("Current tool index: " + currentToolIndex);
        //Debug.Log("Tool 1 is visible: " + tool1.GetComponent<Renderer>().enabled);
        //Debug.Log("Tool 2 is visible: " + tool2.GetComponent<Renderer>().enabled);
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
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

