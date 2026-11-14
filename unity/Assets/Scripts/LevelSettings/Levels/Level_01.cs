using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/01", fileName = "Level_01")]
public class Level_01 : LevelSettings_SO
{

    public override GameController.OutputKind GetOutput(GameController.InputKind input)
    {
        if(input == GameController.InputKind.Input_A) { return GameController.OutputKind.Output_B; }
        else { return GameController.OutputKind.Output_A; }
    }
}
