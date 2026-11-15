using UnityEngine;

[CreateAssetMenu(menuName = "Level/flipping", fileName = "Level Flipping")]
public class Level_03 : LevelSettings_SO
{
    GameController.OutputKind nextOutput = GameController.OutputKind.Output_B;
    public override GameController.OutputKind GetOutput(GameController.InputKind input)
    {
        GameController.OutputKind next = nextOutput;

        if (next == GameController.OutputKind.Output_B) { nextOutput = GameController.OutputKind.Output_A; }
        else { nextOutput = GameController.OutputKind.Output_B; };
        return next;
    }
}
