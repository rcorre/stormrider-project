using UnityEngine;

public class PlayerTurn : State<Battle> {
    public override void Update(Battle battle) {
        base.Update(battle);
        var tile = battle.map.mouse.tileUnderMouse;
        if (tile != null) {
            var battler = tile.battler;
            if (battler != null && battler.alignment == Alignment.Ally) {
                battle.states.Push(new UnitSelected(battler));
            }
        }
    }
}
