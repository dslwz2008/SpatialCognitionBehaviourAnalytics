using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExtractFeatures : MonoBehaviour {

    string dataDir;

    // Use this for initialization
    void Start () {
        dataDir = Application.streamingAssetsPath + "/VROutput";
        string[] files = Directory.GetFiles(dataDir, "*.csv");
        string outputDir = Application.streamingAssetsPath + "/BehaviourFeatures/";

        foreach (string filename in files)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            StreamWriter sw = new StreamWriter(outputDir + name + ".csv");
            string[] lines = File.ReadAllLines(filename);
            //头尾两行参与运算
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] lastLine = lines[i - 1].Split(new char[] { ',' });
                string[] curLine = lines[i].Split(new char[] { ',' });
                string[] nextLine = lines[i + 1].Split(new char[] {','});

                //on x-z plane,轨迹点的高度忽略，认为是在同一个平面上
                Vector3 p1 = new Vector3(float.Parse(lastLine[0]), 0.0f,
                    float.Parse(lastLine[2]));
                Vector3 p2 = new Vector3(float.Parse(curLine[0]), 0.0f,
                    float.Parse(curLine[2]));
                Vector3 p3 = new Vector3(float.Parse(nextLine[0]), 0.0f,
                    float.Parse(nextLine[2]));
                Vector3 moveDir1 = p2 - p1;
                Vector3 moveDir2 = p3 - p2;
                float deltaPos = moveDir1.magnitude;
                float deltaAngle = Mathf.Abs(Vector3.Angle(moveDir1,moveDir2));
                
                //视线方向
                Vector3 viewDir1 = new Vector3(float.Parse(lastLine[3]), float.Parse(lastLine[4]),
                    float.Parse(lastLine[5]));
                Vector3 viewDirOnPlane1 = new Vector3(float.Parse(lastLine[3]), 0.0f,
                    float.Parse(lastLine[5]));
                Vector3 viewDir2 = new Vector3(float.Parse(curLine[3]), float.Parse(curLine[4]),
                    float.Parse(curLine[5]));
                Vector3 viewDirOnPlane2 = new Vector3(float.Parse(curLine[3]), 0.0f,
                    float.Parse(curLine[5]));

                //两个时刻，视线方向的水平夹角的变化量
                float deltaAngleHorizontal = Vector3.Angle(viewDirOnPlane1, viewDirOnPlane2);

                //两个时刻，视线方向的垂直夹角的变化量
                float angleView1Vertical = Vector3.Angle(viewDir1, viewDirOnPlane1);
                float angleView2Vertical = Vector3.Angle(viewDir2, viewDirOnPlane2);
                float deltaAngleVertical = Mathf.Abs(angleView1Vertical - angleView2Vertical);

                //第几行，是从1开始计数的
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", i+1, 
                    curLine[0],curLine[1],curLine[2], 
                    moveDir1.x, moveDir1.y, moveDir1.z,
                    curLine[3], curLine[4], curLine[5],
                    deltaPos.ToString("F6"), 
                    deltaAngle.ToString("F6"),
                    (deltaAngleHorizontal+ deltaAngleVertical).ToString("F6")));
            }
            sw.Close();
        }
        Debug.Log("Compute Finished!");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
