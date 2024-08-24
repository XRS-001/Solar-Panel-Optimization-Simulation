using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class BasicTextLengthLimiter
    {
        public TextMeshProUGUI text;
        public int characterLimit;
        [HideInInspector]
        public bool buttonPressed;
        public string identifier;
    }
    public GameObject eventManager;
    public TextMeshProUGUI latitudeButtonText;
    bool latitudeButtonPressed;
    public TextMeshProUGUI timeButtonText;
    bool timeButtonPressed;
    public TextMeshProUGUI dayButtonText;
    bool dayButtonPressed;
    public BasicTextLengthLimiter[] basicLimitedTexts;
    public SunCalculations sunCalculations;
    public SolarRaySimulation solarRaySimulation;
    public void Button(string name)
    {
        foreach (BasicTextLengthLimiter text in basicLimitedTexts)
        {
            if(name == text.identifier)
            {
                if (text.buttonPressed == false)
                {
                    text.buttonPressed = true;
                }
                else
                {
                    text.buttonPressed = false;
                }
            }
        }
    }
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
    public void TimeButton()
    {
        if (timeButtonPressed)
        {
            timeButtonPressed = false;
        }
        else
        {
            timeButtonPressed = true;
        }
    }
    public void DayButton()
    {
        if (dayButtonPressed)
        {
            dayButtonPressed = false;
        }
        else
        {
            dayButtonPressed = true;
        }
    }
    public void StartSimulating()
    {
        if (!solarRaySimulation.isSimulating)
            solarRaySimulation.StartCoroutine(solarRaySimulation.Simulating());
    }
    public void VisualizeRays()
    {
        if (solarRaySimulation.visualizeRays == false)
            solarRaySimulation.visualizeRays = true;
        else
            solarRaySimulation.visualizeRays = false;
    }
    // Update is called once per frame
    void Update()
    {
        foreach(BasicTextLengthLimiter text in basicLimitedTexts)
        {
            if(text.buttonPressed)
            {
                eventManager.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Backspace) && text.text.text.Length > 0)
                {
                    text.text.text = text.text.text.Substring(0, text.text.text.Length - 1);
                    foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                    {
                        if (limitedText.identifier == solarRaySimulation.rayCountSetting)
                            solarRaySimulation.rayCount = (int)float.Parse(limitedText.text.text);
                    }
                    foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                    {
                        if (limitedText.identifier == solarRaySimulation.spreadAngleSetting)
                            solarRaySimulation.spreadAngle = float.Parse(limitedText.text.text);
                    }
                    foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                    {
                        if (limitedText.identifier == solarRaySimulation.distanceSetting)
                            solarRaySimulation.distance = float.Parse(limitedText.text.text);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    text.buttonPressed = false;
                    eventManager.SetActive(true);
                }
                else if (Input.anyKeyDown)
                {
                    string keyPressed = Input.inputString;
                    float nulled;
                    if ((float.TryParse(keyPressed, out nulled) || (keyPressed == "." && text.text.text.Length == 1)) && text.text.text.Length < text.characterLimit)
                    {
                        text.text.text += keyPressed;
                        foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                        {
                            if (limitedText.identifier == solarRaySimulation.rayCountSetting)
                                solarRaySimulation.rayCount = (int)float.Parse(limitedText.text.text);
                        }
                        foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                        {
                            if (limitedText.identifier == solarRaySimulation.spreadAngleSetting)
                                solarRaySimulation.spreadAngle = float.Parse(limitedText.text.text);
                        }
                        foreach (BasicTextLengthLimiter limitedText in basicLimitedTexts)
                        {
                            if (limitedText.identifier == solarRaySimulation.distanceSetting)
                                solarRaySimulation.distance = float.Parse(limitedText.text.text);
                        }
                    }
                }
            }
        }
        if (latitudeButtonPressed)
        {
            eventManager.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Backspace) && latitudeButtonText.text.Length > 0)
            {
                latitudeButtonText.text = latitudeButtonText.text.Substring(0, latitudeButtonText.text.Length - 1);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                latitudeButtonPressed = false;
                eventManager.SetActive(true);
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
        if (timeButtonPressed)
        {
            eventManager.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Backspace) && timeButtonText.text.Length > 0)
            {
                timeButtonText.text = timeButtonText.text.Substring(0, timeButtonText.text.Length - 1);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                timeButtonPressed = false;
                eventManager.SetActive(true);
            }
            else if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString;
                float number;
                switch (timeButtonText.text.Length)
                {
                    case 0:
                        if(float.TryParse(keyPressed, out number))
                            if(number < 3)
                                timeButtonText.text += keyPressed;
                    break;
                    case 1:
                        if (float.TryParse(keyPressed, out number))
                            timeButtonText.text += keyPressed;
                        break;
                    case 2:
                        if (keyPressed == ":")
                            timeButtonText.text += keyPressed;
                        break;
                    case 3:
                        if (float.TryParse(keyPressed, out number))
                        {
                            if (timeButtonText.text[1] == '4')
                            {
                                if (number == 0)
                                {
                                    timeButtonText.text += "0";
                                    if (timeButtonText.text != "24:00")
                                        sunCalculations.timeInHours = (float)TimeSpan.Parse(timeButtonText.text).TotalHours;
                                    else
                                        sunCalculations.timeInHours = 24;
                                }
                            }
                            else if (number < 7)
                                timeButtonText.text += keyPressed;
                        }
                        break;
                    case 4:
                        if (float.TryParse(keyPressed, out number))
                        {
                            if (timeButtonText.text.EndsWith("6") || timeButtonText.text[1] == '4')
                            {
                                if(number == 0)
                                {
                                    timeButtonText.text += "0";
                                    if (timeButtonText.text != "24:00")
                                        sunCalculations.timeInHours = (float)TimeSpan.Parse(timeButtonText.text).TotalHours;
                                    else
                                        sunCalculations.timeInHours = 24;
                                }
                            }
                            else
                            {
                                timeButtonText.text += keyPressed;
                                if (timeButtonText.text != "24:00")
                                    sunCalculations.timeInHours = (float)TimeSpan.Parse(timeButtonText.text).TotalHours;
                                else
                                    sunCalculations.timeInHours = 24;
                            }
                        }
                        break;
                }
            }
        }
        if (dayButtonPressed)
        {
            eventManager.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Backspace) && dayButtonText.text.Length > 0)
            {
                dayButtonText.text = dayButtonText.text.Substring(0, dayButtonText.text.Length - 1);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                dayButtonPressed = false;
                eventManager.SetActive(true);
            }
            else if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString;
                float number;
                switch (dayButtonText.text.Length)
                {
                    case 0:
                        if (float.TryParse(keyPressed, out number))
                            if (number < 4)
                            {
                                dayButtonText.text += keyPressed;
                                sunCalculations.dayOfYear = (int)float.Parse(dayButtonText.text);
                            }
                        break;
                    case 1:
                        if (float.TryParse(keyPressed, out number))
                            if (dayButtonText.text[0] == '3')
                            {
                                if (number < 7)
                                {
                                    dayButtonText.text += keyPressed;
                                    sunCalculations.dayOfYear = (int)float.Parse(dayButtonText.text);
                                }
                            }
                        break;
                    case 2:
                        if (float.TryParse(keyPressed, out number))
                            if (dayButtonText.text[1] == '6')
                            {
                                if (number < 6)
                                {
                                    dayButtonText.text += keyPressed;
                                    sunCalculations.dayOfYear = (int)float.Parse(dayButtonText.text);
                                }
                            }
                        break;
                }
            }
        }
    }
}
