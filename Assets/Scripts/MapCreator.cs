using UnityEngine;

namespace de.officeryoda {
    public class MapCreator : MonoBehaviour {

        public Vector2Int mapSize;

        [SerializeField] private MapPoint pointPrefab;
        [SerializeField] private float spacing;

        private void Start() {
            createMap();
        }

        private void createMap() {
            if(mapSize.x < 0 || mapSize.y < 0) return;

            // Destroy old childs
            foreach(Transform child in transform) {
                Destroy(child.gameObject);
            }

            // To make it work with a centered parent
            Vector2 offset = new Vector2(mapSize.x, mapSize.y) / -2f + Vector2.one * spacing / 2f;

            for(int x = 0; x < mapSize.x; x++) {
                for(int y = 0; y < mapSize.y; y++) {
                    MapPoint point = Instantiate(pointPrefab, transform);
                    point.transform.position = new Vector2(x, y) * spacing + offset;
                }
            }
        }

        private void OnValidate() {
            if(Application.isPlaying) {
                createMap();
            }

            if(mapSize.x < 0) mapSize.x = 0;
            if(mapSize.y < 0) mapSize.y = 0;
        }
    }
}
