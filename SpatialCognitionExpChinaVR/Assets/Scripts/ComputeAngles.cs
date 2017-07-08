using UnityEngine;
using System.Collections;
using System.IO;

public class ComputeAngles : MonoBehaviour {
    string dataDir;
    Vector3 zeroVector = new Vector3(0.08f, 0.0f, 0.08f);
	// Use this for initialization
	void Start () {
        dataDir = Application.streamingAssetsPath + "/VROutput";
//        dataDir = Application.streamingAssetsPath + "/CommonOutput";
        string[] files = Directory.GetFiles(dataDir, "*.csv");
        string outputDir = Application.streamingAssetsPath + "/VRAngles/";
//        string outputDir = Application.streamingAssetsPath + "/CommonAngles/";
        foreach (string filename in files)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            StreamWriter sw = new StreamWriter(outputDir + name + ".csv");
            string [] lines = File.ReadAllLines(filename);
            for(int i = 1; i < lines.Length; i++)
            {
                string[] lastLine = lines[i-1].Split(new char[] { ',' });
                //on x-z plane
                Vector3 lastPos = new Vector3(float.Parse(lastLine[0]), 0.0f,
                    float.Parse(lastLine[2]));
                Vector3 viewDir = new Vector3(float.Parse(lastLine[3]), float.Parse(lastLine[4]),
                    float.Parse(lastLine[5]));
                Vector3 viewDirOnPlane = new Vector3(float.Parse(lastLine[3]), 0.0f,
                    float.Parse(lastLine[5]));
                string[] curLine = lines[i].Split(new char[] { ',' });
                Vector3 curPos = new Vector3(float.Parse(curLine[0]), 0.0f,
                    float.Parse(curLine[2]));
                Vector3 moveDir = curPos - lastPos;
                //原地不动，默认为0
                float angle1 = 0.0f, angle2 = 0.0f;
                if(Mathf.Abs(moveDir.x) > zeroVector.x ||
                    Mathf.Abs(moveDir.z) > zeroVector.z)
                {
                    angle1 = Vector3.Angle(moveDir, viewDirOnPlane);//移动方向与视线平面方向的夹角
                    angle2 = Vector3.Angle(viewDirOnPlane, viewDir);//视线平面方向与视线方向的夹角
                }

                Vector3 viewDir2 = new Vector3(float.Parse(curLine[3]), float.Parse(curLine[4]),
                    float.Parse(curLine[5]));
                Vector3 viewDirOnPlane2 = new Vector3(float.Parse(curLine[3]), 0.0f,
                    float.Parse(curLine[5]));
                float angle3 = Vector3.Angle(viewDirOnPlane, viewDirOnPlane2);//视线方向的水平夹角的变化量
                float angleView2Height = Vector3.Angle(viewDirOnPlane2, viewDir2);
                float angle4 = Mathf.Abs(angle2 - angleView2Height);//视线方向的垂直夹角的变化量
                sw.WriteLine(string.Format("{0},{1},{2},{3}", angle1.ToString("F6"),angle2.ToString("F6"),
                    angle3.ToString("F6"), angle4.ToString("F4")));
            }
            sw.Close();
        }
        Debug.Log("Finished!");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
