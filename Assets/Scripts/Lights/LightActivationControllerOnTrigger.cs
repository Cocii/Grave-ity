using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightActivationControllerOnTrigger : MonoBehaviour
{
    private Light2D lightComponent;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start() {
        lightComponent = gameObject.GetComponent<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            lightComponent.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            lightComponent.enabled = false;
        }
    }
}
