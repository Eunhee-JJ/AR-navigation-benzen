using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.InputSystem;


public class Navigation : MonoBehaviour
{   
    // public static TMP_Text latitude_text;
    // public static TMP_Text longitude_text;
    // public static TMP_Text altitude_text;
    public TMP_InputField InputStart;
    public TMP_InputField InputEnd;
    public TMP_Text goalLatitude_text;
    public TMP_Text goalLongitude_text;
    public TMP_Text goalAltitude_text;
    public TMP_Text NowSection;
    public TMP_Text GoalSection;
    public GameObject Goal;
    public GameObject _Path;
    public int start;
    public int end;
    static GPS gps;
    static Compass compass;
    static Path path;
 
    public bool pathReceiveOn;
    void Start () {
        // goalLatitude_text = GameObject.Find("GoalLatitude");
        // goalLongitude_text = GameObject.Find("GoalLongitude");
        // goalAltitude_text = GameObject.Find("GoalAltitude");
        Goal = GameObject.Find("Goal");
        gps = GameObject.Find("GPSManager").GetComponent<GPS>();
        compass = GameObject.Find("CompassManager").GetComponent<Compass>();
        _Path = GameObject.Find("Path");
        path = GameObject.Find("Path").GetComponent<Path>();
        pathReceiveOn = true;
        Input.location.Start();
        Input.compass.enabled = true;
        int.TryParse(InputStart.text, out start);
        int.TryParse(InputEnd.text, out end);
        Debug.Log($"{start} {end}");
        path.ReceiveRoute(start, end);
    }


    public static float magneticHeading;
    public static float trueHeading;

    static float latitude;
    static float longitude;
    static float altitude;

    private static WaitForSeconds second;
 
    private static bool gpsStarted = false;
    
    // Update is called once per frame
    void Update()
    {
        latitude = GPS.latitude;
        longitude = GPS.longitude;
        altitude = GPS.altitude;
        // 화면 정중앙에 화살표 배치
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, Camera.main.nearClipPlane + 1));
        
        // 화살표 회전
        Vector2 origin = new Vector2 (latitude, longitude);
        double goalN = 0f; // latitude(N)
        double goalS = 0f; // longitude(S)
        if (path.goal_floor != path.now_floor && path.goal_section == path.now_section){
            int num = path.NearestStair(latitude, longitude);
            goalN = path.curves[num].y;
            goalS = path.curves[num].x;
        }
        else (goalN, goalS) = path.GetGoalXY(latitude, longitude, altitude);
        Vector2 goalVector = new Vector2((float)goalN-latitude, (float)goalS-longitude);
        // Vector3 goal = new Vector3(37.29437556536372f,126.9749498578987f,10.0f);
        GoalSection.text = "Goal Section: "+ path.goal_section.ToString();
        NowSection.text = "Now Section: " +path.now_section.ToString();
        // 굴곡점에서 현재 층과 목표 층이 다를 경우 화살표 아래/위로 회전
        double range = 0.00015;
        if (path.goal_floor != path.now_floor && goalN-range <= latitude && latitude <= goalN+range && goalS-range <= longitude && longitude <= goalS+range){
            if (path.goal_floor > path.now_floor ){
                transform.rotation = Quaternion.Euler(0,0,0);
                
            }
            else if(path.goal_floor < path.now_floor){
                transform.rotation = Quaternion.Euler(90,0,0);
            }
            
        }
        // 화살표 좌/우로 회전
        else{
            
            goalLatitude_text.text = "Goal N: " + goalN.ToString();
            goalLongitude_text.text = "Goal S: " + goalS.ToString();
            
            if (Input.compass.headingAccuracy == 0 || Input.compass.headingAccuracy > 0) {
                //냅다 북쪽 보기
                magneticHeading = Input.compass.magneticHeading;
                trueHeading = Input.compass.trueHeading;

                Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                Quaternion compass = Quaternion.Euler(75, Input.compass.trueHeading, 0);

                Quaternion north = Quaternion.Euler(75, cameraRotation.eulerAngles.y - compass.eulerAngles.y, 0);
                //transform.rotation = north;

                // 목표 좌표 보기
                Quaternion goalRotation =  Quaternion.LookRotation(new Vector2(90-latitude,0), goalVector);
                transform.rotation = Quaternion.Euler(75, north.y+goalRotation.eulerAngles.y, 0);
            }
            
        }
        
        if(path.arrived == true){
            gameObject.SetActive(false);
            _Path.gameObject.SetActive(false);
        }
        
    }

}
