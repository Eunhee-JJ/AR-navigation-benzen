using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using TMPro;


public struct POINT
{
    public double x{get; set;}
    public double y{get; set;}

    public POINT(double X, double Y){
        x=X;
        y=Y;
    }
};

public class Path : MonoBehaviour
{   
    
    // -----------루트 탐색---------------
    // [DllImport ("__Internal",  CharSet = CharSet.Unicode )]
    // private static extern IntPtr FindRoute (int A, int B, out int size);
    Backtracking _Backtracking;
    public TMP_Text RouteSize;
    public bool arrived;
    public int[,] section_data= new int[14,4] { {0,0,0,0},{1,2,3,4},{3,4,5,6},{5,6,7,8},{10,11,12,13},{8,9,10,11},{8,9,14,15},{14,15,16,17},{17,18,19,20},{17,18,21,22},{21,22,23,24},{26,27,28,29},{24,25,26,27},{24,25,30,31} };
    public POINT[] pt_data = new POINT[32] { new POINT(0,0),new POINT(126.9754602196598,37.29478198598422), new POINT(126.9754752405957,37.29458202680357), new POINT(126.9749883592608,37.2942825303303), new POINT(126.9748731914428,37.29447227262472), new POINT(126.9745980002353,37.29451769391698), new POINT(126.9743487111332,37.29439159568935), new POINT(126.9744804336599,37.29480378325461), new POINT(126.9746674755936,37.29479185294591), new POINT(126.9745245019564,37.29494591237214), new POINT(126.9750871779494,37.29524582322077), new POINT(126.9752060760335,37.29508134246644), new POINT(126.9758021252003,37.29499251134501), new POINT(126.9758333488903,37.29519293688357), new POINT(126.974174540252,37.29498329476842), new POINT(126.9739176481061,37.2948630772268), new POINT(126.9740649565264,37.29527846453868), new POINT(126.9742475004085,37.29525511309688), new POINT(126.9741062046268,37.29541971339564), new POINT(126.9745989480694,37.29567092665127), new POINT(126.9747266243117,37.29552707426124), new POINT(126.9737921467492,37.29546584414163), new POINT(126.9735326113664,37.29534217720906), new POINT(126.9736565894222,37.29573924678559), new POINT(126.9738417244103,37.2957069248873), new POINT(126.9737129217353,37.29586614010423), new POINT(126.9743867227178,37.29624402422568), new POINT(126.9744400202724,37.29603632713705), new POINT(126.9750237472092,37.29596832447017), new POINT(126.9750693890494,37.29618147442766), new POINT(126.9733489095936,37.29593692678846), new POINT(126.9733108445274,37.29579340991676) };
    public POINT[] curves = new POINT[14] { new POINT(0,0),
                                     new POINT(126.9749498578987, 37.29437556536372),
                                     new POINT(126.9744405477215, 37.29448208879217),
                                     new POINT(126.9745439828286, 37.29485639847768),
                                     new POINT(126.9740186425546, 37.29492853101725),
                                     new POINT(126.97413305208, 37.29531868605105),
                                     new POINT(126.9736909333054, 37.29539587834776),
                                     new POINT(126.9737496672004,37.29578308280982),
                                     new POINT(126.9733660836855,37.29586102122261),
                                     new POINT(126.9744172281713,37.29613552072814),
                                     new POINT(126.9750305577834,37.29606820951162),
                                     new POINT(126.9751981159524,37.29517739432703),
                                     new POINT(126.9755171604642,37.29505022842142),
                                     new POINT(126.975399484522,37.29458978629793)
                                    };
    public bool isinner(POINT _CurrentPos, List<POINT> _vVertex)
    {
        int iSize = _vVertex.Count;
        int iCount = 0;

        for (int i = 0; i < iSize; ++i)
        {
            int j = (i + 1) % iSize;

            if (_CurrentPos.y > _vVertex[i].y != _CurrentPos.y > _vVertex[j].y)
            {
                double[] S = new double[3];
                S[0] = _vVertex[j].x - _vVertex[i].x;
                S[1] = _vVertex[j].y - _vVertex[i].y;
                S[2] = _CurrentPos.y - _vVertex[i].y;

                double itsPosX = S[0] * S[2] / S[1] + _vVertex[i].x;
                if (_CurrentPos.x < itsPosX)
                    ++iCount;
            }
        }
        return (iCount % 2) != 0;
    }

    // 행선지 층 구별
    public int goal_floor_classification(int num){ 
        int floor = -1;

        if (num <= 10){
            floor = 1;
        }
        else if (num <= 23){
            floor = 2;
        }
        else if (num <= 36){
            floor = 3;
        }

        return floor;
    }

    // 행선지 섹션 구별
    public int goal_section_classification(int num){
        
        int section = -1;

        if (num == 0){
            section = 0;
        }
        else if (num == 1 || num == 11 || num == 24){
            section = 1;
        }
        else if (num == 2 || num == 12 || num == 25){
            section = 2;
        }
        else if (num == 13 || num == 26){
            section = 3;
        }
        else if (num == 3 || num == 16 || num == 29){
            section = 4;
        }
        else if (num == 4 || num == 15 || num == 28){
            section = 5;
        }
        else if (num == 5 || num == 14 || num == 27){
            section = 6;
        }
        else if (num == 17 || num == 30){
            section = 7;
        }
        else if (num == 6 || num == 18 || num == 31){
            section = 8;
        }
        else if (num == 7 || num == 19 || num == 32){
            section = 9;
        }
        else if (num == 20 || num == 33){
            section = 10;
        }
        else if (num == 8 || num == 22 || num == 35){
            section = 11;
        }
        else if (num == 9 || num == 21 || num == 34){
            section = 12;
        }
        else if (num == 10 || num == 23 || num == 36){
            section = 13;
        }

        return section;
    }

    // 현재 층 구별
    public int location_floor_classification(double altitude){ 
        int floor = -1;

        if (altitude <= 17){
            floor = 1;
        }
        else if (altitude <= 34){
            floor = 2;
        }
        else {
            floor = 3;
        }

        return floor;
    }

    // 현재 섹션 구별
    public int location_section_classification(double latitude, double longitude){
        // POINT ptPos = new POINT(126.974165,37.295996);
        POINT ptPos = new POINT(longitude,latitude);
        int section = 0;

        for (int i = 1; i < 14; i++) {
            List<POINT> vV = new List<POINT>();
            for (int a = 0; a < 4; a++)
                vV.Add(pt_data[section_data[i,a]]);

            if (isinner(ptPos, vV)) {
                section = i;
                // printf("%d", section);
                return section;
            }
        }

        return section;
    }

    // 굴곡점 좌표 획득
    public (double, double) get_curve(int sec1, int sec2){
        double latitude=0;
        double longitude=0;

        if (sec1 == 1 && sec2 == 2 || sec1 == 2 && sec2 == 1 || sec1 == 0 && (sec2 == 1 || sec2 == 2)){
            longitude = curves[1].x;
            latitude = curves[1].y;
        }
        else if (sec1 == 2 && sec2 == 3 || sec1 == 3 && sec2 == 2 || sec1 == 0 && (sec2 == 2)){
            longitude = curves[2].x;
            latitude = curves[2].y;
        }
        else if (sec1 == 3 && sec2 == 6 || sec1 == 6 && sec2 == 3 || sec1 == 5 && sec2 == 6 || sec1 == 6 && sec2 == 5  || sec1 == 0 && (sec2 == 5 || sec2 == 6)){
            longitude = curves[3].x;
            latitude = curves[3].y;
        }
        else if (sec1 == 6 && sec2 == 7 || sec1 == 7 && sec2 == 6 || sec1 == 0 && (sec2 == 6)){
            longitude = curves[4].x;
            latitude = curves[4].y;
        }
        else if (sec1 == 7 && sec2 == 9 || sec1 == 9 && sec2 == 7 || sec1 == 8 && sec2 == 9 || sec1 == 9 && sec2 == 8  || sec1 == 0 && (sec2 == 8 || sec2 == 9)){
            longitude = curves[5].x;
            latitude = curves[5].y;
        }
        else if (sec1 == 9 && sec2 == 10 || sec1 == 10 && sec2 == 9 || sec1 == 0 && (sec2 == 9)){
            longitude = curves[6].x;
            latitude = curves[6].y;
        }
        else if (sec1 == 10 && sec2 == 13 || sec1 == 13 && sec2 == 10 || sec1 == 12 && sec2 == 13 || sec1 == 13 && sec2 == 12 || sec1 == 0 && (sec2 == 12 || sec2 == 13)){
            longitude = curves[7].x;
            latitude = curves[7].y;
        }
        else if (sec1 == 13 && sec2 == 0 || sec1 == 0 && sec2 == 13){
            longitude = curves[8].x;
            latitude = curves[8].y;
        }
        else if (sec1 == 11 && sec2 == 12 || sec1 == 12 && sec2 == 11 || sec1 == 0 && (sec2 == 11 || sec2 == 12)){
            longitude = curves[9].x;
            latitude = curves[9].y;
        }
        else if (sec1 == 11 && sec2 == 0 || sec1 == 0 && sec2 == 11){
            longitude = curves[10].x;
            latitude = curves[10].y;
        }
        else if (sec1 == 4 && sec2 == 5 || sec1 == 5 && sec2 == 4  || sec1 == 0 && (sec2 == 4 || sec2 == 5)){
            longitude = curves[11].x;
            latitude = curves[11].y;
        }
        else if (sec1 == 4 && sec2 == 0 || sec1 == 0 && sec2 == 4){
            longitude = curves[12].x;
            latitude = curves[12].y;
        }
        else if (sec1 == 1 && sec2 == 0 || sec1 == 0 && sec2 == 1){
            longitude = curves[13].x;
            latitude = curves[13].y;
        }
        // else if (sec1==0){
        //     Location li = Input.location.lastData;
        //     latitude = li.latitude;
        //     longitude = li.longitude;
        // }
        Debug.Log($"{sec1} {sec2} {latitude} {longitude}");
        return (latitude, longitude);
    }
    public int NearestStair(double latitude, double longitude){
        double d=0;
        double min=1;
        int minIndex=-1;
        for(int i=1; i<14; i++){
            d = Math.Pow(longitude-curves[i].x,2)-Math.Pow(latitude-curves[i].y,2);
            if (d<min){
                min = d;
                minIndex = i;
            }
        }
        return minIndex;
    }
    void Start(){
        _Backtracking = GameObject.Find("Backtracking").GetComponent<Backtracking>();
    }
    public int[] route;
    public int i;
    public bool received;
    public void ReceiveRoute(int start, int end){
        int size = 0;
        received = false;
        _Backtracking.FindRoute(start, end, out size);
        //Array.Copy(_Backtracking.result_path, route, size);
        route = _Backtracking.result_path;
        received = true;
        Debug.Log($"{route[1]}");
        RouteSize.text = "Length of Route: "+size.ToString();
        // IntPtr _receiveData = _Backtracking.result_path;
        // route = new int[size];
        // if(size > 1) received = true;
        // RouteSize.text = size.ToString();
        // Debug.Log($"{size}");
        // Marshal.Copy(_receiveData, route, 0, route.Length);
    }

    public int goal_floor;
    public int goal_section;
    public int now_floor;
    public int now_section;
    public double goal_latitude;
    public double goal_longitude;
    public double goal_altitude;
    public int[] floorZ = {10, 27, 44};
    public int cnt;

    // 화살표가 가리킬 방향 좌표(굴곡점) 구하기
    public (double, double) GetGoalXY(double latitude, double longitude, double altitude)
    {
        if (i+1 >= route.Length){ // 도착점 도착 시
                // 도착 ui 띄우기
                arrived = true;
                gameObject.SetActive(false);
            }
        now_floor = location_floor_classification(altitude);
        now_section = location_section_classification(latitude, longitude);
        if(cnt == 0 && now_section != route[cnt]) i--;
        goal_floor = goal_floor_classification(route[i+1]);
        goal_section = goal_section_classification(route[i+1]);
        Debug.Log($"goal: {route[i+1]}");
        Debug.Log($"goal_floor goal_section: {goal_floor} {goal_section}");
        
        Debug.Log($"now_floor now_section: {now_floor} {now_section}");
        (goal_latitude, goal_longitude) = get_curve(now_section, goal_section);
        Debug.Log($"goal_XY: {goal_latitude} {goal_longitude}");
        // 같은 섹션 다른 층일 때
        
        // 아닐 때
        //else{
            (goal_latitude, goal_longitude) = get_curve(now_section, goal_section);
        //}
        // 포인트 도착 시
        if( goal_floor == now_floor && goal_section == now_section){
            i++;
            cnt++;
            Debug.Log($"{i} {goal_floor} {goal_section}");

        }
        return (goal_latitude, goal_longitude);
    }
}
