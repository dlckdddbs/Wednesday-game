using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 버튼을 연결할 변수
    public Button startButton;
    public Button optionButton;
    public Button shopButton;
    private UnityAction action;
    void Start()
    {
        // UnityAction을 사용한 이벤트 연결 방식
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);
        // 무명 메서드를 활용한 이벤트 연결 방식
        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });
        // 람다식을 활용한 이벤트 연결 방식
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }
    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
    
}
