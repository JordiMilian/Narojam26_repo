using UnityEngine;

public class GoalMeter_Controller : MonoBehaviour
{
    public float maxScale;

    public float goalScale;
    [SerializeField] Transform blackSquare;

    public void OnUpdatedMigajas()
    {
        float normalizedMigajas = Mathf.InverseLerp( GameController.instance.CurrentLevel.migajas_goal, 0, GameController.instance.CurrentMigajas);
        goalScale = Mathf.Lerp(0, maxScale, normalizedMigajas);
    }
    private void Update()
    {
        float lerpedvalue = Mathf.Lerp(blackSquare.transform.localScale.y, goalScale, 0.1f);
        blackSquare.transform.localScale = new Vector3(blackSquare.transform.localScale.x, lerpedvalue, 1);
    }
}
