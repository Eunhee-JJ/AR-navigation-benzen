using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasAppear : MonoBehaviour
{
    public Button GO;
    public GameObject canvas;
    public GameObject canvas2;
    public GameObject Arrow;
    Path path;
    Navigation navi;
    public TMP_InputField InputStart;
    public TMP_InputField InputEnd;
    public bool isShowing;
    public bool isShowing2;
    public bool isShowing3;
    // Start is called before the first frame update
    void Start()
    {
        canvas2 = GameObject.Find("Canvas2");
        Arrow = GameObject.Find("Arrow");
        navi = Arrow.GetComponent<Navigation>();
        path = GameObject.Find("Path").GetComponent<Path>();
        //Arrow.SetActive(false);
        isShowing = true;
        isShowing2 = false;
        isShowing3 = false;
        path.arrived = false;
        GO.onClick.AddListener(on_off);
    }

    void on_off(){
        if(InputStart.text != "" && InputEnd.text != ""){
            isShowing = !isShowing;
            isShowing2 = !isShowing2;
            isShowing3 = !isShowing3;
        }
        navi.start = int.Parse(InputStart.text);
        navi.end = int.Parse(InputEnd.text);
        Debug.Log($"{isShowing} {isShowing2}");
    }

    // Update is called once per frame
    void Update()
    {
        canvas.SetActive(isShowing);
        Arrow.GetComponent<Navigation>().enabled = isShowing3;
        canvas2.SetActive(isShowing2);
    }
}
