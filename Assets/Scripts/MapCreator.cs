using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace de.officeryoda {
    public class MapCreator : MonoBehaviour {

        public Vector2Int mapSize;

        [SerializeField] private MapPoint pointPrefab;
        [SerializeField] private float spacing;


        private void Start() {
            CreateMap();
        }

        private void CreateMap() {
            if(mapSize.x <= 0 || mapSize.y <= 0) return;

            // Destroy old childs
            foreach(Transform child in transform) {
                Destroy(child.gameObject);
            }

            // To make it work with a centered parent
            Vector2 offset = new Vector2(mapSize.x, mapSize.y) / -2f + Vector2.one * spacing / 2f;

            MapPoint[,] mapPoints = new MapPoint[mapSize.x, mapSize.y];

            for(int x = 0; x < mapSize.x; x++) {
                for(int y = 0; y < mapSize.y; y++) {
                    MapPoint point = Instantiate(pointPrefab, transform);
                    point.transform.position = new Vector2(x, y) * spacing + offset;

                    mapPoints[x, y] = point;
                }
            }

            AssignNeighbours(mapPoints);
        }

        private void AssignNeighbours(MapPoint[,] mapPoints) {
            int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
            int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };

            for(int x = 0; x < mapSize.x; x++) {
                for(int y = 0; y < mapSize.y; y++) {
                    MapPoint point = mapPoints[x, y];
                    HashSet<MapPoint> neighbours = new();

                    for(int i = 0; i < 8; i++) {
                        int nx = x + dx[i];
                        int ny = y + dy[i];

                        if(nx >= 0 && nx < mapSize.x && ny >= 0 && ny < mapSize.y) {
                            neighbours.Add(mapPoints[nx, ny]);
                        } else {
                            neighbours.Add(point);
                        }
                    }

                    point.neighbours = neighbours.ToList();
                }
            }
        }

        private void OnValidate() {
            if(Application.isPlaying) {
                CreateMap();
            }

            if(mapSize.x < 0) mapSize.x = 0;
            if(mapSize.y < 0) mapSize.y = 0;
        }
    }
}
