using UnityEngine;
using System.Collections.Generic;

public class Battle : MonoBehaviour {
    public string dataName;
    public List<string> playerCharacters;

    public StateMachine<Battle> states { get; private set; }
    public TileMap map { get; private set; }
    public List<Battler> npcs { get; private set; }
    public List<Battler> allies { get; private set; }

    void Start() {
	map = FindObjectOfType<TileMap>();
	map.GenerateMap();

        var battleData = DataManager.LoadOnce<BattleData>(dataName);

        npcs = new List<Battler>();
        allies = new List<Battler>();

	// add npcs to map
        foreach (var npc in battleData.npcs) {
	    var npcData = battleData.npcData[npc.index];
	    var tile = map.TileAt(npc.row, npc.col);
	    npcs.Add(Battler.Create(npcData, Alignment.Enemy, tile));
        }

	// add player characters to deploy points
        for (int i = 0; i < playerCharacters.Count && i < battleData.deployPoints.Length; i++) {
	    var name = playerCharacters[i];
	    var coord = battleData.deployPoints[i];
            var charData = DataManager.Fetch<Character>(name);
	    var tile = map.TileAt(coord.row, coord.col);
	    allies.Add(Battler.Create(charData, Alignment.Ally, tile));
        }

        states = new StateMachine<Battle>(this, new PlayerTurn());
    }

    void Update() {
        states.Update();
    }
}
