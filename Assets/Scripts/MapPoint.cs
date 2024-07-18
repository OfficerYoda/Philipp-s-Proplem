using UnityEngine;

namespace de.officeryoda {
    [RequireComponent(typeof(SpriteRenderer))]
	public class MapPoint : MonoBehaviour {

		public Color color = Color.white;

        private SpriteRenderer rend;

        private void Start() {
            rend = GetComponent<SpriteRenderer>();
        }

        private void OnValidate() {
            if(!rend) {
                rend = GetComponent<SpriteRenderer>();
            }
            rend.color = color;
        }
    }
}
