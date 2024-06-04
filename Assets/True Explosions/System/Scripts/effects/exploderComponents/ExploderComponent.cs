using UnityEngine;
using System.Collections;

public abstract class ExploderComponent : MonoBehaviour {
	public abstract void onExplosionStarted(Exploder2 exploder);
	void Start() {
		// is needed to have the ability to disable the components
	}
}
