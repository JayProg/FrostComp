using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoldierPanelScreenLogic : MonoBehaviour {
    public Text Name, SoldierType, Move, Fight, Shoot, Armor, Will, HealthText, NotesText;
    public Slider HealthBarRef;
    public Button EditButtonRef;

    [HideInInspector]public Character SoldierRef;

	void Start () {
        Name.text = SoldierRef.Name;
        SoldierType.text = SoldierRef.SoldierType;
        Move.text = SoldierRef.GetStat("Move").Value + "";
        Fight.text = SoldierRef.GetStat("Fight").Value + "";
        Shoot.text = SoldierRef.GetStat("Shoot").Value + "";
        Armor.text = SoldierRef.GetStat("Armor").Value + "";
        Will.text = SoldierRef.GetStat("Will").Value + "";

        NotesText.text = SoldierRef.Notes == null ? "" :  SoldierRef.Notes;

        HealthBarRef.maxValue = SoldierRef.GetStat("Health").Value;
        HealthBarRef.value = SoldierRef.CurHealth;
        HealthText.text = HealthBarRef.value + " / " + HealthBarRef.maxValue;

        EditButtonRef.onClick.AddListener(() => { SendToEdit(PlayerManager.CurrentPlayer.Soldiers.IndexOf(SoldierRef)); });
    }
	
	public void IncreaseHealth()
    {
        SoldierRef.CurHealth = Mathf.Clamp(SoldierRef.CurHealth + 1, 0, SoldierRef.GetStat("Health").Value);
        HealthBarRef.value = SoldierRef.CurHealth;
        HealthText.text = HealthBarRef.value + " / " + HealthBarRef.maxValue;
        PlayerManager.SavePlayer();
    }
    public void DecreaseHealth()
    {
        SoldierRef.CurHealth = Mathf.Clamp(SoldierRef.CurHealth - 1, 0, SoldierRef.GetStat("Health").Value);
        HealthBarRef.value = SoldierRef.CurHealth;
        HealthText.text = HealthBarRef.value + " / " + HealthBarRef.maxValue;
        PlayerManager.SavePlayer();
    }

    public void SendToEdit(int id)
    {
        var editScreen = FindObjectOfType<EditPartyScreenLogic>();
        editScreen.CurrentCharacterInfo = PlayerManager.CurrentPlayer.Soldiers[id];
        editScreen.OnEnable();
    }
}