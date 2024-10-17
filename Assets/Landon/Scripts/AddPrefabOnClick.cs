using UnityEngine;
using UnityEngine.UI;

public class AddPrefabOnClick : MonoBehaviour {

    [SerializeField] GameObject prefab;
    RectTransform layoutPanel;

	void Start() {
        layoutPanel = transform.parent.GetComponent<RectTransform>();
	}

	public void InstatiateAbove() {
        //instatiate the prefab as a child of your own parent
        Instantiate(prefab, transform.parent);
        //move yourself to be last in the hierarchy
        transform.SetAsLastSibling();
        //rebuild layout so that the layout groups update to fit the new instatiation
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }
}