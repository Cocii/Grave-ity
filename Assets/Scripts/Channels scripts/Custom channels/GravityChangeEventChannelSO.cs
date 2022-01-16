using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public abstract class GravityChangeEventChannelAbstractSO : DescriptionBaseSO {
	public abstract IEnumerator LateRaiseEventVoidCo();
}

[CreateAssetMenu(menuName = "Events/Gravity Change Event Channel")]
public class GravityChangeEventChannelSO : GravityChangeEventChannelAbstractSO {

	public UnityAction<GravityChangesEnum> OnEventRaised;
	public UnityAction OnEventRaisedVoid;

	public void RaiseEvent(GravityChangesEnum value) {
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

	public void RaiseEventVoid() {
		if (OnEventRaisedVoid != null)
			OnEventRaisedVoid.Invoke();
	}

	public override IEnumerator LateRaiseEventVoidCo() {
		yield return new WaitForEndOfFrame();
		RaiseEventVoid();
		yield break;
	}

}


