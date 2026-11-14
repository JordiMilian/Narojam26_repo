using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Apostador_Controller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isOverhead = false;
    public int currentBet, maxBet;
    [SerializeField] TextMeshProUGUI tmp_bet;
    [SerializeField] GameController.OutputKind outputKind;
    [SerializeField] Transform blackSquare;
    [SerializeField] float blackSquare_maxScale, goalValue;
    private void Awake()
    {
        GameController.instance.clickAction.action.started += OnPointerDown;
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
                //trigger animació apretat
                //spawnejar migaja
                //update UI
            }
            else
            {
                Debug.Log("couldnt place bet");
                //trigger animacio error
            }
        }
    }
    bool PlaceBet()
    {
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

        float normalizedMigajas = Mathf.InverseLerp(GameController.instance.CurrentLevel.migajas_goal, 0, GameController.instance.CurrentMigajas);
        goalValue = Mathf.Lerp(0, blackSquare_maxScale, normalizedMigajas);

        //migajas
    }

    private void Update()
    {
        float lerpedvalue = Mathf.Lerp(blackSquare.transform.localScale.y, goalValue, 0.1f);
        blackSquare.transform.localScale = new Vector3(blackSquare.transform.localScale.x, lerpedvalue, 1);
    }
}
