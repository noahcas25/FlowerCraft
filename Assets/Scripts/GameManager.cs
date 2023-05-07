using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _pot;
    [SerializeField] private Transform _orderPot;
    [SerializeField] private char[] _bundleLetters = new char[6];
    [SerializeField] private ParticleSystem _particles;

    private Transform[] _flowerBundles, _orderFlowerBundles;

    public bool _roundActive = false;
    private int _keyCounter;
    private float _ordersFulfilled;

    private char _keyPressed;
    private string _charSequence, _orderSequence;

    public static GameManager Instance {get; private set;}
 
    private void Awake() => Instance = this;

    private void Start() {
        _flowerBundles = SeperateBundles(_pot);
        _orderFlowerBundles = SeperateBundles(_orderPot);

        Order();
        UIManager.Instance.NewRound();
    }

    private void Update() {
        Round();
    }

    private void Round() {
        if(!_roundActive) return;

        Keys();
        
        if(_keyCounter >= 3)
            RoundOver();
    }
    
    private void Keys() {
        if(!Input.anyKeyDown || string.IsNullOrEmpty(Input.inputString)) return;

        _keyPressed = Input.inputString[0];

        if(char.IsLetter(_keyPressed)) {
            _keyPressed = char.ToUpper(_keyPressed);

            SelectFlower(_keyPressed);
            UIManager.Instance.InputText(_keyCounter, _keyPressed);
            _keyCounter++;
       }
    } 
    
    private void SelectFlower(char input) {
        for(int i = 0; i < _bundleLetters.Length; i++) {
            if(input.Equals(_bundleLetters[i])) {
                _flowerBundles[i].GetChild(_keyCounter).gameObject.SetActive(true);
                _charSequence += _bundleLetters[i];
            }
        }
    }

    private void Order() {
        int num;

        for(int i = 0; i < 3; i++) {
            num = Random.Range(0, 6);
            _orderFlowerBundles[num].GetChild(i).gameObject.SetActive(true);
            _orderSequence += _bundleLetters[num];
        }

    }

    private void CompareOrder() {
         if(_charSequence.Equals(_orderSequence)) {
            _particles.Play();
            _ordersFulfilled++;

            UIManager.Instance.FillStar(_ordersFulfilled);
            AudioManager.Instance.PlayAudio(0);

            if(_ordersFulfilled == 25) {
                UIManager.Instance.GameEnd();
                AudioManager.Instance.PlayAudio(1);
            }
                
         } else {
            UIManager.Instance.WrongAnswer();
             AudioManager.Instance.PlayAudio(2);
         }
    }

    private void RoundOver() {
        SetRoundActive(false);
        StartCoroutine(RoundOverDelay());
    }

    private void Reset() {
        _keyCounter = 0;
        _charSequence = "";
        _orderSequence = "";
        _particles.Stop();
        
        foreach (Transform child in _flowerBundles) {
            foreach(Transform grandChild in child){
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in _orderFlowerBundles) {
            foreach(Transform grandChild in child){
                grandChild.gameObject.SetActive(false);
            }
        }
    }

    public void SetRoundActive(bool value) => _roundActive = value;
    
    private Transform[] SeperateBundles(Transform pot) {
        var count = pot.childCount;

        Transform[] bundles = new Transform[count];
        for(int i = 0; i < count; i++) {
            bundles[i] = pot.GetChild(i);
        }

        return bundles;
    }

    private IEnumerator RoundOverDelay() {

        CompareOrder();
        yield return new WaitForSeconds(1.5f);

        Reset();
        UIManager.Instance.NewRound();

        Order();
    }
}
