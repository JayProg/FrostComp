using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public enum MagicSchool { Chronomancer, Elementalist, Enchanter, Illusionist, Necromancer, Sigilist, Soothsayer, Summoner, Thaumaturge, Witch }
public class SpellLibraryScreenLogic : MonoBehaviour
{
    public RectTransform SpellButtonTemplate;
    public Text DropdownTemplate;
    Text activeDropdown;
    List<Spell> masterSpellList = new List<Spell>();
    List<Transform> uiList = new List<Transform>();

    void Awake()
    {
        prepareListData();
    }

    void OnEnable()
    {
        DrawSpellButtons(masterSpellList);
    }

    public void FilterBySchool(string school)
    {
        DrawSpellButtons(GetSpellsFromSchool(school));
    }

    List<Spell> GetSpellsFromSchool(string school)
    {
        var spells = new List<Spell>();

        foreach (var spell in masterSpellList)
        {
            if(spell.School == school || school == null || school == "")
            {
                spells.Add(spell);
            }
        }

        return spells;
    }

    void ClearUi()
    {
        foreach (var ui in uiList)
        {
            Destroy(ui.gameObject);
        }

        uiList.Clear(); 
    }

    void DrawSpellButtons(List<Spell> spells)
    {
        ClearUi();

        SpellButtonTemplate.gameObject.SetActive(true);
        DropdownTemplate.gameObject.SetActive(true);

        for (int i = 0; i < spells.Count; i++)
        {
            var newButton = (RectTransform)Instantiate(SpellButtonTemplate, SpellButtonTemplate.position, SpellButtonTemplate.rotation, SpellButtonTemplate.parent);
            newButton.GetComponentInChildren<Text>().text = spells[i].Name + " | " + spells[i].School + " | " + spells[i].RollAmount;
            newButton.SetAsLastSibling();

            var spell = spells[i];
            newButton.Find("Button").GetComponent<Button>().onClick.AddListener(delegate () { AddSpellToPlayer(spell); });

            var dropdown = (Text)Instantiate(DropdownTemplate, DropdownTemplate.transform.position, DropdownTemplate.transform.rotation, DropdownTemplate.transform.parent);
            dropdown.text = spells[i].Description;
            dropdown.transform.SetAsLastSibling();
            dropdown.gameObject.SetActive(false);

            newButton.GetComponent<Button>().onClick.AddListener(delegate () { ToggleText(dropdown); });

            uiList.Add(newButton.transform);
            uiList.Add(dropdown.transform);
        }
        SpellButtonTemplate.gameObject.SetActive(false);
        DropdownTemplate.gameObject.SetActive(false);

    }

    public void AddSpellToPlayer(Spell spell)
    {
        PlayerManager.CurrentPlayer.SpellList.Add(spell);
        PlayerManager.SavePlayer();
    }

    private void prepareListData()
    {

        var castTypes = new List<string>();

        castTypes.Add("Out of Game");
        castTypes.Add("Line of Sight");
        castTypes.Add("Touch");
        castTypes.Add("Self Only");
        castTypes.Add("Area Effect");

        int index = 0;
        try
        {
            var path = "file:///" + Application.streamingAssetsPath + "/Spells.txt";
#if Android
    path = "jar:file://" + Application.dataPath + "!/assets/Spells.txt"
#endif
            WWW www = new WWW(path);
            while(!www.isDone)
            {

            }
            // StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            TextAsset text = Resources.Load<TextAsset>("Spells");
            var splitFile = text.text.Split('\n');
            string line = "";
            line = splitFile[0];
            int splitter = 0;
            int i = 0;
            while ( i < splitFile.Length && (line = splitFile[i++]) != null)
            {
                foreach (string castType in castTypes)
                {
                    if (line.Contains(castType))
                    {
                        splitter = line.IndexOf(castType);
                        break;
                    }
                }


                string spellName = line.Substring(0, splitter - 2);
                string description = line.Substring(splitter);
                /*
                var newButton = (RectTransform)Instantiate(SpellButtonTemplate, SpellButtonTemplate.position, SpellButtonTemplate.rotation, SpellButtonTemplate.parent);
                newButton.GetComponentInChildren<Text>().text = spellName;
                newButton.SetAsLastSibling();
               
                
                var dropdown = (Text)Instantiate(DropdownTemplate, DropdownTemplate.transform.position, DropdownTemplate.transform.rotation, DropdownTemplate.transform.parent);
                dropdown.text = description;
                dropdown.transform.SetAsLastSibling();
                dropdown.gameObject.SetActive(false);

                newButton.GetComponent<Button>().onClick.AddListener(delegate () { ToggleText(dropdown); });
                */
                var spellSplit = spellName.Split(' ');
                var spellInfo = new Spell();
                int count = 0;
                var name = "";
                while (count++ < 10)
                {
                    if(spellSplit[count] == "/")
                    {
                        spellInfo.School = spellSplit[count - 1];
                        for (int x = 0; x <= count - 2; x++)
                        {
                            name += spellSplit[x] + " ";
                        }
                        break;
                    }
                }
                spellInfo.Name = name;
                spellInfo.RollAmount = int.Parse(spellSplit[count + 1]);
                spellInfo.Description = description;

                for (int x = 0; x < castTypes.Count; x++)
                {
                    if(description.Substring(0, 1) == castTypes[x].Substring(0,1))
                    {
                        spellInfo.Category = castTypes[x];
                    }
                    spellInfo.Description.Replace(castTypes[x], "");
                }

               // uiList.Add(newButton.transform);
               // uiList.Add(dropdown.transform);

                masterSpellList.Add(spellInfo);
            }

        }
        catch (IOException e)
        {
            print(e.InnerException);
        }

        SpellButtonTemplate.gameObject.SetActive(false);
        DropdownTemplate.gameObject.SetActive(false);
    }

    void ToggleText(Text text)
    {
        text.gameObject.SetActive(!text.gameObject.activeSelf);

        if (text.gameObject.activeSelf)
        {
            if (activeDropdown) activeDropdown.gameObject.SetActive(false);
            activeDropdown = text;
        }
        
    }
}

[System.Serializable]
public class Spell
{
    public string Name, Description, Category, School;
    public int RollAmount;
}