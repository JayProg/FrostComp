using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchLayoutToHeight : MonoBehaviour {
    LayoutElement layout;
    RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        layout = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        layout.minHeight = rectTransform.sizeDelta.y;
	}
}
