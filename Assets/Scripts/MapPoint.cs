using UnityEngine;

namespace de.officeryoda {
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapPoint : MonoBehaviour {

        public Color color = Color.white;
        private SpriteRenderer rend;

        public MapPoint[] neighbours;

        public bool drawGizmos;

        private void Start() {
            rend = GetComponent<SpriteRenderer>();
        }

        private void OnValidate() {
            if(!rend) {
                rend = GetComponent<SpriteRenderer>();
            }
            rend.color = color;
        }

        private void OnDrawGizmos() {
            if(!drawGizmos) return;

            foreach(MapPoint neighbour in neighbours) {
                Vector2 center = (transform.position + neighbour.transform.position) / 2f;
                Gizmos.DrawSphere(center, 0.05f);
            }
        }

        private void OnDrawGizmosSelected() {
            foreach(MapPoint neighbour in neighbours) {
                Vector2 center = (transform.position + neighbour.transform.position) / 2f;
                Gizmos.DrawSphere(center, 0.05f);
            }
        }
    }
}
