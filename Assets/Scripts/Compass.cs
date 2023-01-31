using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

 
public class Compass : MonoBehaviour {
    public GameObject GPSManager;
    public static Compass instance;
    public static float magneticHeading;
    public static float trueHeading;
 
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }

    void Start(){
        StartCoroutine(Compass_On());
    }
 
    public IEnumerator Compass_On () {
        Input.location.Start();
        Input.compass.enabled = true; //나침반 활성화
        while (true) {
 
            //헤딩 값 가져오기
            if (Input.compass.headingAccuracy == 0 || Input.compass.headingAccuracy > 0) {
                magneticHeading = Input.compass.magneticHeading;
                trueHeading = Input.compass.trueHeading;
            }
 
            yield return new WaitForSeconds (0.1f);
        }
    }
}