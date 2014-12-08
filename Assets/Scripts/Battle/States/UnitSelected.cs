using UnityEngine;

public class UnitSelected : State<Battle> {
    private Battler _battler;

    public UnitSelected(Battler battler) {
        _battler = battler;
    }

    public override void Update(Battle battle) {
        base.Update(battle);
        var tile = battle.map.mouse.tileUnderMouse;
        if (tile != null) {
            var battler = tile.battler;
        }
    }
}
