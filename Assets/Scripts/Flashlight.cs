using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light spotlight;
    private bool isOn = false;

    void Start()
    {
        spotlight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            spotlight.enabled = isOn;
        }
    }
}
