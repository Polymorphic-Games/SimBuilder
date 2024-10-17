using UnityEngine;
using UnityEngine.UI;

public class RemoveOnClick : MonoBehaviour {
	//temporary sloppy code
	public void Remove() {
		Destroy(transform.parent.gameObject);
		//LayoutRebuilder.ForceRebuildLayoutImmediate(GreatGreatGrandma);
	}
}