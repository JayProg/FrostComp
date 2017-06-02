using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class EditorScripts : MonoBehaviour {

    [MenuItem("Frostgrave/Load Beasts Into Dropdown")]
    static void LoadBeasts()
    {
        var beastiaryFile = Resources.Load<TextAsset>("Beastiary");

        var line = "";
        StringReader reader = new StringReader(beastiaryFile.text);
        var options = new List<string>();
        while ((line = reader.ReadLine()) != null)
        {
            var spaceSplitter = line.Split(' ');

            var beastName = "";
           
            for (int i = 0; spaceSplitter[i] != "M" && i < 100; i++)
            {
                beastName += spaceSplitter[i] + " ";
            }

            if (options.Count == 0)
            {
                options.Add(beastName);
                continue;
            }

            for (int i = 0; i < options.Count; i++)
            {
                if(i == (options.Count - 1) || options[i].Substring(0,1).CompareTo(beastName.Substring(0,1)) > 0)
                {
                    options.Insert(i, beastName.Trim());
                    break;
                }
            }
            
        }

        Selection.activeGameObject.GetComponent<Dropdown>().AddOptions(options);
    }
    [MenuItem("Frostgrave/Create Beast Files")]
    static void CreateBeastJson()
    {
        var beastiaryFile = Resources.Load<TextAsset>("Beastiary");

        var line = "";
        StringReader reader = new StringReader(beastiaryFile.text);

        var characters = new List<Character>();
        while((line = reader.ReadLine()) != null)
        {
            var spaceSplitter = line.Split(' ');
            var character = new Character();

            var beastName = "";
            for (int i = 0; spaceSplitter[i] != "M" && i < 100; i++)
            {
                beastName += spaceSplitter[i] + " ";
            }

            var statCount = 0;
            for (statCount = 0; spaceSplitter[statCount] != "Notes" && statCount < 50; statCount++)
            {}

            character.GetStat("Move").Value = int.Parse(spaceSplitter[statCount + 1]);
            character.GetStat("Fight").Value = int.Parse(spaceSplitter[statCount + 2]);
            character.GetStat("Shoot").Value = int.Parse(spaceSplitter[statCount + 3]);
            character.GetStat("Armor").Value = int.Parse(spaceSplitter[statCount + 4]);
            character.GetStat("Will").Value = int.Parse(spaceSplitter[statCount + 5]);
            character.GetStat("Health").Value = int.Parse(spaceSplitter[statCount + 6]);

            character.SoldierType = spaceSplitter[statCount + 7];
            if (line.Contains(",")) character.Notes = line.Substring(line.IndexOf(',') + 2);

            character.CurHealth = character.GetStat("Health").Value;
            character.Name = beastName;

            Debug.Log(beastName + "M: "+ character.GetStat("Move").Value + " Health: " + character.GetStat("Health").Value);
            Debug.Log(character.Notes);

            File.WriteAllText(beastName.Trim() + ".txt", JsonUtility.ToJson(character));

        }
    }
}
