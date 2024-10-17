using UnityEngine;
using UnityEngine.UI;

public class SimpleColorCycle : MonoBehaviour {

    [SerializeField] Color[] colors;
    [SerializeField] int index;
    Image indicator;

	void Start() {
        indicator = GetComponent<Image>();
        RandomColor();
	}

	public void NextColor() {
        index++;
        if (index >= colors.Length) index = 0;
        indicator.color = colors[index];
    }
    public void RandomColor() {
        index = Random.Range(0, colors.Length - 1);
        indicator.color = colors[index];
    }
}