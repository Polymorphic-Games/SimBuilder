using UnityEngine;

public class ExpandPanelToggle : MonoBehaviour
{

	RectTransform panel;
	public Vector2 heightRange = new Vector2(55f, 800f);
	bool open = true;
	float activeSmooth = 0f;

	void Start()
	{
		panel = GetComponent<RectTransform>();
		panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, open ? heightRange.y : heightRange.x);
	}
	private void FixedUpdate()
	{
		float CurrentActiveValue = open ? 1f : 0f;
		activeSmooth = (4 * activeSmooth + CurrentActiveValue) / 5;
		panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(heightRange.x, heightRange.y, activeSmooth));
	}
	public void Toggle()
	{
		open = !open;
	}
}