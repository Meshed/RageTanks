using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour
{
    public GameObject[] clouds;
    public GameObject[] nearHills;
    public GameObject[] farHills;
    public float cloudLayerSpeedModifier;
    public float nearHillLayerSpeedModifier;
    public float farHillLayerSpeedModifier;
    public Camera myCamera;

    private Vector3 lastCamPos;

	// Use this for initialization
	void Start ()
	{
	    lastCamPos = myCamera.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 currCamPos = myCamera.transform.position;
	    float xPosDiff = lastCamPos.x - currCamPos.x;

        adjustParallaxPositionsForArray(clouds, cloudLayerSpeedModifier, xPosDiff);
        adjustParallaxPositionsForArray(nearHills, nearHillLayerSpeedModifier, xPosDiff);
        adjustParallaxPositionsForArray(farHills, farHillLayerSpeedModifier, xPosDiff);

	    lastCamPos = myCamera.transform.position;
	}

    void adjustParallaxPositionsForArray(GameObject[] layerArray, float layerSpeedModifier, float xPosDiff)
    {
        for (int i = 0; i < layerArray.Length; i++)
        {
            Vector3 objPos = layerArray[i].transform.position;

            objPos.x += xPosDiff*layerSpeedModifier;
            layerArray[i].transform.position = objPos;
        }
    }
}
