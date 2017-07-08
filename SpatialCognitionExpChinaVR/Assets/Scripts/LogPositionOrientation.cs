using UnityEngine;
using System.Collections;
using FastFileLog;

public class LogPositionOrientation : MonoBehaviour {
    public float interval = 1.0f;
    float time = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > interval)
        {
            //Debug.Log(gameObject.transform.position.ToString("F6"));
            //Debug.Log(gameObject.transform.forward.ToString("F6"));
            //Debug.Log(JsonUtility.ToJson(gameObject.transform.position));
            time = 0;
            LogManager.Log(gameObject, string.Format("position: {0}; eulerAngles: {1}",
                gameObject.transform.position.ToString("F6"),
                gameObject.transform.eulerAngles.ToString("F6")));
        }
    }
}
