using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;
using System.Text;

public class CommonDataAnalysis : MonoBehaviour {
    public bool showUI = true;
    public LineRenderer walkPath;
    public LineRenderer viewPath;
    public GameObject pointPrefab;
    public GameObject intersectParent;
    public GameObject parent;
    private string dataDir;
    private string outputDir;
    private Dictionary<string, DataTable> dataDict = new Dictionary<string, DataTable>();
    private int selGridInt = 0;
    private Vector2 scrollPosition;
    private List<string> selStrings = new List<string>();

    void Awake() {
        dataDir = Application.streamingAssetsPath + "/Common";
        outputDir = Application.streamingAssetsPath + "/CommonOutput";
        string[] files = Directory.GetFiles(dataDir,"*FirstPersonCharacter*.txt");
        foreach (string filename in files)
        {
            //Debug.Log(filename);
            string key = Path.GetFileNameWithoutExtension(filename);
            //string dateString = key.Substring(key.IndexOf('_') + 1, 16);
            string pairedFilename = filename.Replace("FirstPersonCharacter", "FPSController");

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Time",typeof(string)));
            dt.Columns.Add(new DataColumn("CamPositionX",typeof(float)));
            dt.Columns.Add(new DataColumn("CamPositionY", typeof(float)));
            dt.Columns.Add(new DataColumn("CamPositionZ", typeof(float)));
            dt.Columns.Add(new DataColumn("CamEulerAnglesX", typeof(float)));
            dt.Columns.Add(new DataColumn("CamEulerAnglesY", typeof(float)));
            dt.Columns.Add(new DataColumn("CamEulerAnglesZ", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlPositionX", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlPositionY", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlPositionZ", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlEulerAnglesX", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlEulerAnglesY", typeof(float)));
            dt.Columns.Add(new DataColumn("CtlEulerAnglesZ", typeof(float)));

            string[] lines = File.ReadAllLines(filename);
            string[] pairedLines = File.ReadAllLines(pairedFilename);
            //第一行和最后一行不要
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string line = lines[i];
                string pairedLine = pairedLines[i];
                string timeString;
                string[] position, eularAngles;
                GetSplitItems(line, out timeString, out position, out eularAngles);

                DataRow newRow = dt.NewRow();
                newRow["Time"] = timeString;
                newRow["CamPositionX"] = float.Parse(position[0]);
                newRow["CamPositionY"] = float.Parse(position[1]);
                newRow["CamPositionZ"] = float.Parse(position[2]);
                newRow["CamEulerAnglesX"] = float.Parse(eularAngles[0]);
                newRow["CamEulerAnglesY"] = float.Parse(eularAngles[1]);
                newRow["CamEulerAnglesZ"] = float.Parse(eularAngles[2]);

                GetSplitItems(pairedLine, out timeString, out position, out eularAngles);
                newRow["CtlPositionX"] = float.Parse(position[0]);
                newRow["CtlPositionY"] = float.Parse(position[1]);
                newRow["CtlPositionZ"] = float.Parse(position[2]);
                newRow["CtlEulerAnglesX"] = float.Parse(eularAngles[0]);
                newRow["CtlEulerAnglesY"] = float.Parse(eularAngles[1]);
                newRow["CtlEulerAnglesZ"] = float.Parse(eularAngles[2]);

                dt.Rows.Add(newRow);
            }
            dataDict.Add(key, dt);
            selStrings.Add(key);
        }
	}

    void GetSplitItems(string inputLine, out string timeString, out string[] position, out string[] eularAngles)
    {
        timeString = inputLine.Substring(0, 8);
        //第一对括号
        int leftBracket = inputLine.IndexOf('(', 0);
        int rightBracket = inputLine.IndexOf(')', leftBracket);
        string pos = inputLine.Substring(leftBracket + 1, rightBracket - 1 - leftBracket);
        position = pos.Split(new char[] { ',' });
        //第二队括号
        leftBracket = inputLine.IndexOf('(', rightBracket);
        rightBracket = inputLine.IndexOf(')', leftBracket);
        string angles = inputLine.Substring(leftBracket + 1, rightBracket - 1 - leftBracket);
        eularAngles = angles.Split(new char[] { ',' });
    }

    void Start()
    { }

    // Update is called once per frame
    void Update () {
	
	}

    void OnGUI()
    {
        if (!showUI) { return; }
        string[] strings = selStrings.ToArray();
        //GUILayout.BeginVertical("Box",GUILayout.Height(500));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(200), GUILayout.Height(300));
        selGridInt = GUILayout.SelectionGrid(selGridInt, strings, 1);

        //GUILayout.EndVertical();
        GUILayout.EndScrollView();
        if (GUILayout.Button("确定"))
        {
            Debug.Log("You chose " + selStrings[selGridInt]);
            RenderPlayerPath(selStrings[selGridInt]);
        }
    }

    private void RenderPlayerPath(string key)
    {
        //先清理一下
        if (parent.transform.childCount != 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }
        if (intersectParent.transform.childCount != 0)
        {
            for (int i = 0; i < intersectParent.transform.childCount; i++)
            {
                Destroy(intersectParent.transform.GetChild(i).gameObject);
            }
        }
        walkPath.SetVertexCount(0);
        viewPath.SetVertexCount(0);

        DataTable dt = dataDict[key];
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> intersectPositions = new List<Vector3>();
        StreamWriter sw = new StreamWriter(outputDir + "/" + key + ".csv", false, Encoding.UTF8);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow row = dt.Rows[i];
            Vector3 vec = new Vector3(float.Parse(row["CamPositionX"].ToString()),
                float.Parse(row["CamPositionY"].ToString()), 
                float.Parse(row["CamPositionZ"].ToString()));
            positions.Add(vec);
            //创建节点
            GameObject go = (GameObject)Instantiate(pointPrefab, vec, 
                Quaternion.Euler(new Vector3(90.0f,0.0f,0.0f)));
            go.GetComponent<TextMesh>().text = (i+1).ToString();
            go.transform.parent = parent.transform;
            //视线移动轨迹
            Vector3 eulerAngle = new Vector3(float.Parse(row["CamEulerAnglesX"].ToString()),
                 float.Parse(row["CamEulerAnglesY"].ToString()),
                 float.Parse(row["CamEulerAnglesZ"].ToString()));
            //Debug.Log(eulerAngle);
            Vector3 dir = Quaternion.Euler(eulerAngle) * Vector3.forward;
            string line = String.Format("{0},{1},{2},{3},{4},{5}",
                vec[0].ToString("F6"), vec[1].ToString("F6"), vec[2].ToString("F6"),
                dir[0].ToString("F6"), dir[1].ToString("F6"), dir[2].ToString("F6"));
            sw.WriteLine(line);
            //Debug.Log(dir);
            //Debug.DrawRay(vec, dir, Color.green, 1000, true);
            RaycastHit hit;
            int layerMask = 1 << 8;
            if (Physics.Raycast(vec, dir, out hit, 500f, layerMask))
            {
                //Debug.Log(vec.ToString()+ hit.point.ToString());
                Debug.DrawLine(vec, hit.point, Color.blue, 1000, true);
                intersectPositions.Add(hit.point);
                //创建相交节点
                GameObject intersectGo = (GameObject)Instantiate(pointPrefab, hit.point,
                    Quaternion.identity);
                intersectGo.GetComponent<TextMesh>().text = (i + 1).ToString();
                intersectGo.transform.parent = intersectParent.transform;
                //intersectGo.transform.GetChild(0).LookAt();
            }
        }
        sw.Close();
        viewPath.SetVertexCount(intersectPositions.Count);
        viewPath.SetPositions(intersectPositions.ToArray());
        walkPath.SetVertexCount(positions.Count);
        walkPath.SetPositions(positions.ToArray());
    }
}
