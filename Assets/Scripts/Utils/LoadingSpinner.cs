using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    public RectTransform mainIcon;
    public float timeStep;
    public float oneStepAngle;

    private float startTime;
    private Vector3 iconAngle;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= timeStep)
        {
            iconAngle = mainIcon.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }
}
