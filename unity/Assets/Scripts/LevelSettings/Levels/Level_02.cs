using UnityEngine;

[CreateAssetMenu(menuName = "Level/02", fileName = "Level_02")]
public class Level_02 : LevelSettings_SO
{
    GameController.InputKind lastInput = GameController.InputKind.Input_C;
    public override GameController.OutputKind GetOutput(GameController.InputKind input)
    {
        if(lastInput == input) { lastInput = input; return GameController.OutputKind.Output_B; }
        else { lastInput = input; return GameController.OutputKind.Output_A; }

        
    }
}
