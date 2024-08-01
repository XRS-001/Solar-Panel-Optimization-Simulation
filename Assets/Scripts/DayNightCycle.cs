using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    public Transform sun;
    public float secondsPassedInDay;
    // Update is called once per frame
    void Update()
    {
        sun.rotation = Quaternion.Euler(Mathf.Lerp(-90, 270, secondsPassedInDay / 86400), sun.rotation.y, sun.rotation.z);
    }
}
