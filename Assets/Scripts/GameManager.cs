using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI latitudeButtonText;
    public bool latitudeButtonPressed;
    public SunCalculations sunCalculations;
    public SolarRaySimulation solarRaySimulation;
    public void LatitudeButton()
    {
        if (latitudeButtonPressed)
        {
            latitudeButtonPressed = false;
        }
        else
        {
            latitudeButtonPressed = true;
        }
    }
    public void StartSimulating()
    {
        if (!solarRaySimulation.isSimulating)
            solarRaySimulation.StartCoroutine(solarRaySimulation.Simulating());
    }
    // Update is called once per frame
    void Update()
    {
        if (latitudeButtonPressed)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) && latitudeButtonText.text.Length > 0)
            {
                latitudeButtonText.text = latitudeButtonText.text.Substring(0, latitudeButtonText.text.Length - 1);
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                latitudeButtonPressed = false;
            }
            else if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString;
                float nulled;
                if ((float.TryParse(keyPressed, out nulled) || keyPressed == "." || keyPressed == "-") && ((latitudeButtonText.text.Length < 2 || keyPressed == ".") || (latitudeButtonText.text.Length < 4 && latitudeButtonText.text.Contains(".")) || ((latitudeButtonText.text.Length < 3 || keyPressed == ".") && latitudeButtonText.text.Contains("-")) || (latitudeButtonText.text.Length < 5 && latitudeButtonText.text.Contains(".") && latitudeButtonText.text.Contains("-"))))
                {
                    latitudeButtonText.text += keyPressed;
                    sunCalculations.latitude = float.Parse(latitudeButtonText.text);
                }
            }
        }
    }
}
