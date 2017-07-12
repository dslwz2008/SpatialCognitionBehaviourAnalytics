using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RenderIntersectionPoints : MonoBehaviour
{
    private string intersectPointsDir;
    public GameObject pointPrefab;
    public GameObject intersectParent;
    private Dictionary<string, List<List<float>>> allPoints;

	// Use this for initialization
	void Start () {
        intersectPointsDir = Application.streamingAssetsPath + "/IntersectPoints";
        allPoints = new Dictionary<string, List<List<float>>>();
	    ReadPoints();
	    RenderAllPoints();
	}

    private void RenderAllPoints()
    {
        foreach (KeyValuePair<string, List<List<float>>> pair in allPoints)
        {
            if (pair.Key == "Bounds") continue;
            foreach (List<float> items in pair.Value)
            {
                Vector3 position = new Vector3(items[0],items[1],items[2]);
                Vector3 normal = new Vector3(items[3],items[4],items[5]);
                //创建相交节点
                GameObject intersectGo = (GameObject)Instantiate(pointPrefab, position, Quaternion.identity);
                intersectGo.transform.forward = normal;
                intersectGo.transform.parent = intersectParent.transform;
            }
        }
    }

    private void ReadPoints()
    {
        string[] files = Directory.GetFiles(intersectPointsDir, "*.csv");
        foreach (string filename in files)
        {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] items = lines[i].Trim().Split(new char[] {','});
//                Debug.Log(string.Format("{0},{1},{2},{3}",items[0],items[1],items[2],items[3]));
                if (!allPoints.ContainsKey(items[0]))
                {
                    allPoints[items[0]] = new List<List<float>>();
                }
                List<float> nums = new List<float>();
                nums.Add(float.Parse(items[1]));
                nums.Add(float.Parse(items[2]));
                nums.Add(float.Parse(items[3]));
                nums.Add(float.Parse(items[4]));
                nums.Add(float.Parse(items[5]));
                nums.Add(float.Parse(items[6]));
                allPoints[items[0]].Add(nums);
            }
        }
    }

    // Update is called once per frame
	void Update () {
		
	}
}
