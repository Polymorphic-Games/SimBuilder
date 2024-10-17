using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonScript : MediatorComponent
{

	public bool State { get; private set; } = true;

	[SerializeField] GameObject ImageTrue;
	[SerializeField] GameObject imageFalse;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Toggle);
	}

	public void Toggle()
	{
		State = !State;
		ImageTrue.SetActive(State);
		imageFalse.SetActive(!State);

		if (mediator != null)
		{
			mediator.Notify(this);
		} else
		{
			Debug.Log("Mediator not set");
		}
	}
}

