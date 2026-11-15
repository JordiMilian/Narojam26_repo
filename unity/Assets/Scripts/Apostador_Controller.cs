using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Apostador_Controller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isOverhead = false;
    public int currentBet, maxBet;
    [SerializeField] TextMesh tmp_bet;
    [SerializeField] GameController.OutputKind outputKind;
    [SerializeField] Transform blackSquare;
    [SerializeField] float blackSquare_maxScale, goalValue;
    [SerializeField] float shakeDuration;
    Animator _animator;
    public bool isDisabled;
    [SerializeField] AudioSource button_true, button_fail;
    private void Awake()
    {
        GameController.instance.clickAction.action.started += OnPointerDown;
        _animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOverhead = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOverhead = false;
    }
    public void OnPointerDown(InputAction.CallbackContext ctx)
    {
        if (isOverhead)
        {
            if(PlaceBet())
            {
                Debug.Log("placed bet");
                _animator.SetTrigger("press_true");
                button_true.Play();
                //trigger animació apretat
                //spawnejar migaja
                //update UI
            }
            else
            {
                Debug.Log("couldnt place bet");
                _animator.SetTrigger("press_false");
                button_fail.Play();
                //trigger animacio error
            }
        }
    }
    bool PlaceBet()
    {
        if (isDisabled) { return false; }
        if (GameController.instance.CurrentMigajas == 0) { return false; }
        if (currentBet >= maxBet) { return false; }

        currentBet++;
        GameController.instance.CurrentMigajas--;

        UpdateUI();

        return true;
    }
    public void OnWonBet()
    {
        GameController.instance.CurrentMigajas += currentBet * 2;
        StartCoroutine(C_winBetCoroutine());
        currentBet = 0;
        UpdateUI();
    }
    public void OnLostBet()
    {
        currentBet = 0;
        UpdateUI();
    }


    public void UpdateUI()
    {
        string newString = currentBet + "/" + maxBet;
        tmp_bet.text = newString;

        float normalizedMigajas = Mathf.InverseLerp(maxBet, 0, currentBet);
        goalValue = Mathf.Lerp(0, blackSquare_maxScale, normalizedMigajas);

        //migajas
    }

    private void Update()
    {
        float lerpedvalue = Mathf.Lerp(blackSquare.transform.localScale.y, goalValue, 0.1f);
        blackSquare.transform.localScale = new Vector3(blackSquare.transform.localScale.x, lerpedvalue, 1);
    }
    public IEnumerator C_winBetCoroutine()
    {
        _animator.SetBool("win",true);
        transform.DOShakePosition(shakeDuration, .06f, 20);

        yield return new WaitForSeconds(shakeDuration);
        _animator.SetBool("win", false);
        yield return new WaitForSeconds(1);
    }
}
