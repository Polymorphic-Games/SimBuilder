using TMPro;
using UnityEngine;

public class UpdateTextOnValueChange : MonoBehaviour {

    [SerializeField] TextMeshProUGUI text;

    public void UpdateText() {
        text.text = GetComponent<TMP_InputField>().text;
	}
}