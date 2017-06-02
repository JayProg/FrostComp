using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyWarbandScreenLogic : MonoBehaviour {
    public Transform SoldierPanelRef, AddButtonRef;
    List<Transform> uiList = new List<Transform>();

	void OnEnable () {
        SoldierPanelRef.gameObject.SetActive(true);
        ClearList();
        foreach (var soldier in PlayerManager.CurrentPlayer.Soldiers)
        {
            var panel = Instantiate(SoldierPanelRef, SoldierPanelRef.position, SoldierPanelRef.rotation, SoldierPanelRef.parent) as Transform;
            panel.GetComponent<SoldierPanelScreenLogic>().SoldierRef = soldier;

            uiList.Add(panel.transform);
        }
        AddButtonRef.SetAsLastSibling();

        SoldierPanelRef.gameObject.SetActive(false);
	}

    void ClearList()
    {
        foreach (var item in uiList)
        {
            Destroy(item.gameObject);
        }
        uiList.Clear();
    }
}