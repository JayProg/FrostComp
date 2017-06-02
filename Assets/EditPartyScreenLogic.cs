using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class EditPartyScreenLogic : MonoBehaviour {
    public static string CHARACTER_KEY = "Char_Save_";
    public Text[] StatValueTexts;
    [HideInInspector]public Character CurrentCharacterInfo;
    [SerializeField] Dropdown soldierTypeDropdown, beastDropdown;
    [SerializeField] InputField NameInput;

    void Awake()
    {
        //LoadSoldierDefaults();
        if (soldierTypeDropdown) soldierTypeDropdown.onValueChanged.AddListener(OnSoldierValueChanged);
        if(beastDropdown)beastDropdown.onValueChanged.AddListener(OnBeastValueChanged);
        NameInput.onValueChanged.AddListener(OnTextValueChanged);
    }

    public void OnEnable()
    {
        if(soldierTypeDropdown) LoadCharacterIntoUi(soldierTypeDropdown.options[0].text);

        UpdateStatUi();
    }

    void OnDisable()
    {
        CurrentCharacterInfo = null;
    }

    public void IncreaseStat(string name)
    {
        CurrentCharacterInfo.GetStat(name).Value++;
        UpdateStatUi();
    }

    public void DecreaseStat(string name)
    {
        CurrentCharacterInfo.GetStat(name).Value--;
        UpdateStatUi();
    }

    void SetStat(string name, int value)
    {
        CurrentCharacterInfo.GetStat(name).Value = value;
        UpdateStatUi();
    }

    void UpdateStatUi()
    {
        if (CurrentCharacterInfo == null) return;

        for (int i = 0; i < CurrentCharacterInfo.Stats.Length; i++)
        {
            transform.Find(CurrentCharacterInfo.Stats[i].Name + "/StatNumberText").GetComponent<Text>().text = CurrentCharacterInfo.GetStat(CurrentCharacterInfo.Stats[i].Name).Value + "";
        }

        NameInput.text = CurrentCharacterInfo.Name;
        transform.Find("NotesField").GetComponent<InputField>().text = CurrentCharacterInfo.Notes == null ? "" : CurrentCharacterInfo.Notes;
    }

    public void OnNotesValueChanged(string newText)
    {
        CurrentCharacterInfo.Notes = newText;
        PlayerManager.SavePlayer();
    }

    void OnSoldierValueChanged(int value)
    {
        LoadCharacterIntoUi(soldierTypeDropdown.options[value].text);
    }
    void OnBeastValueChanged(int value)
    {
        LoadCharacterIntoUi(beastDropdown.options[value].text);
    }

    void OnTextValueChanged(string value)
    {
        CurrentCharacterInfo.Name = value;
    }

    public void PreloadSoldierStats(string name)
    {
        LoadCharacterIntoUi(name);
    }

    public void LoadCharacterIntoUi(string name)
    {
        var json = Resources.Load<TextAsset>(name).text;
        var character = JsonUtility.FromJson<Character>( json );
        if (character != null)
        {
            CurrentCharacterInfo = character;
            UpdateStatUi();
        } 
        else
        {
            Debug.Log("No character Found");
        }
    }
    /*
    public void SaveCharacter()
    {
        CurrentCharacterInfo.SoldierType = soldierTypeDropdown.options[soldierTypeDropdown.value].text;
        CurrentCharacterInfo.CurHealth = CurrentCharacterInfo.GetStat("Health").Value;

        var json = JsonUtility.ToJson(CurrentCharacterInfo);
        PlayerPrefs.SetString(CHARACTER_KEY + CurrentCharacterInfo.Name, json);
        PlayerPrefs.Save();
    }
    */
    public void AddCharacterToParty()
    {
        CurrentCharacterInfo.CurHealth = CurrentCharacterInfo.GetStat("Health").Value;

        PlayerManager.CurrentPlayer.Soldiers.Add(CurrentCharacterInfo);
        PlayerManager.SavePlayer();
    }

    public void RemoveCharacterFromParty()
    {
        PlayerManager.CurrentPlayer.Soldiers.Remove(CurrentCharacterInfo);
        PlayerManager.SavePlayer();
    }

    void LoadSoldierDefaults()
    {
        /*
        foreach (var item in soldierTypeDropdown.options)
        {
            var json = Resources.Load<TextAsset>(item.text).text;
            PlayerPrefs.SetString(CHARACTER_KEY + item.text, json);
            PlayerPrefs.Save();
        }
        */
        LoadCharacterIntoUi(soldierTypeDropdown.options[0].text);
    }

    void SaveSoldiers()
    {
        foreach (var item in soldierTypeDropdown.options)
        {
            var json = PlayerPrefs.GetString(CHARACTER_KEY + item.text);
            File.WriteAllText(item.text + ".txt", json);
        }
    }


}