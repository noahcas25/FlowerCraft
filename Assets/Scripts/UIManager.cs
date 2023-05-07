using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] _letterText;
    [SerializeField] private GameObject _startCover, _endCover;
    [SerializeField] private GameObject _orderView, _keys;
    [SerializeField] private Image _star;
    
    private bool _pressable = true;
    
    public static UIManager Instance {get; private set;}

    private void Awake() => Instance = this;


    public void Begin() {
        if(_pressable && !GameManager.Instance._roundActive) {
            _pressable = false;
            GameManager.Instance.SetRoundActive(true);
            LeanTween.moveLocal(_orderView, new Vector3(0f, -1350f, 0f), 0.25f);
        }
    }


    public void InputText(int index, char keyPressed) {
        _letterText[index].text = keyPressed + "";
    }

    public void FillStar(float value) {
        _star.fillAmount = value/25;
    }

    public void WrongAnswer() {
        LeanTween.moveSplineLocal(_keys, new Vector3[]{new Vector3(0f,-454f,0f), new Vector3(20f,-454f,0f), new Vector3(-10f,-454f,0f), new Vector3(20f,-454f,0f), new Vector3(0f,-454f,0f), new Vector3(0f,-454f,0f)}, 1f);
    }

    public void NewRound() {
        foreach(TextMeshProUGUI tm in _letterText)
            tm.text = "";

       LeanTween.moveLocal(_orderView, new Vector3(0f, 0f, 0f), 0.25f);
       StartCoroutine(PressDelay());
    }

    public void GameStart() {
        _startCover.SetActive(false);
    }

    public void GameEnd() {
        _endCover.SetActive(true);
    }

    public void Continue() {
       _endCover.SetActive(false);
    }

    private IEnumerator PressDelay() { 
        yield return new WaitForSeconds(0.5f);
        _pressable = true;
    }

    public void Quit() => Application.OpenURL("https://nightmodegames.itch.io/");
}
