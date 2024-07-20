using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace de.officeryoda {
    public class MapCreator : MonoBehaviour {

        public Vector2Int mapSize;

        [SerializeField] private MapPoint pointPrefab;
        [SerializeField] private float spacing;
        [SerializeField] private float maxOffset;

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
                    Vector2 randomOffset = new(Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset));
                    MapPoint point = Instantiate(pointPrefab, transform);
                    point.transform.position = new Vector2(x, y) * spacing + offset + randomOffset;

                    mapPoints[x, y] = point;
                }
            }

            AssignNeighbours(mapPoints);
            AssignIntermediatePoints(mapPoints);
        }

        private void AssignNeighbours(MapPoint[,] mapPoints) {
            int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
            int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };

            // Point between four MapPoints
            for(int x = 0; x < mapSize.x; x++) {
                for(int y = 0; y < mapSize.y; y++) {
                    MapPoint point = mapPoints[x, y];
                    MapPoint[] neighbours = new MapPoint[8];

                    for(int i = 0; i < 8; i++) {
                        int nx = x + dx[i];
                        int ny = y + dy[i];

                        if(nx >= 0 && nx < mapSize.x && ny >= 0 && ny < mapSize.y) {
                            neighbours[i] = mapPoints[nx, ny];
                        } else {
                            neighbours[i] = point;
                        }

                        point.neighbours = neighbours;
                    }
                }
            }
        }

        private void AssignIntermediatePoints(MapPoint[,] mapPoints) {
            /*
             * Array index-positions
             * 6 5 4
             * 7 x 3
             * 0 1 2
             */

            // Point between four MapPoints
            // P   P
            //   i
            // P   P
            for(int x = 0; x < mapSize.x - 1; x++) {
                for(int y = 0; y < mapSize.y - 1; y++) {

                    MapPoint bl = mapPoints[x, y];
                    MapPoint br = mapPoints[x + 1, y];
                    MapPoint tl = mapPoints[x, y + 1];
                    MapPoint tr = mapPoints[x + 1, y + 1];

                    Vector2 intermediate = IntermediatePoint(bl, br, tl, tr);

                    bl.neighbourPoints[4] = intermediate;
                    br.neighbourPoints[6] = intermediate;
                    tl.neighbourPoints[2] = intermediate;
                    tr.neighbourPoints[0] = intermediate;
                }
            }

            // Point between two points horizontal
            // P i P
            for(int x = 0; x < mapSize.x - 1; x++) {
                for(int y = 0; y < mapSize.y; y++) {

                    MapPoint left = mapPoints[x, y];
                    MapPoint right = mapPoints[x + 1, y];

                    Vector2 intermediate = IntermediatePoint(left, right);

                    left.neighbourPoints[3] = intermediate;
                    right.neighbourPoints[7] = intermediate;
                }
            }

            // Point between two points vertical
            // P
            // i
            // P
            for(int x = 0; x < mapSize.x; x++) {
                for(int y = 0; y < mapSize.y - 1; y++) {

                    MapPoint bot = mapPoints[x, y];
                    MapPoint top = mapPoints[x, y + 1];

                    Vector2 intermediate = IntermediatePoint(top, bot);

                    bot.neighbourPoints[5] = intermediate;
                    top.neighbourPoints[1] = intermediate;
                }
            }

            // Handle Edge Points
            // Top + Bottom Edge
            for(int x = 0; x < mapSize.x; x++) {
                MapPoint top = mapPoints[x, mapSize.y - 1];
                Vector2 topPos = top.transform.position;
                top.neighbourPoints[4] = topPos;
                top.neighbourPoints[5] = topPos;
                top.neighbourPoints[6] = topPos;

                MapPoint bot = mapPoints[x, 0];
                Vector2 botPos = bot.transform.position;
                bot.neighbourPoints[0] = botPos;
                bot.neighbourPoints[1] = botPos;
                bot.neighbourPoints[2] = botPos;
            }

            // Left + Right Edge
            for(int y = 0; y < mapSize.y; y++) {
                MapPoint left = mapPoints[0, y];
                Vector2 leftPos = left.transform.position;
                left.neighbourPoints[6] = leftPos;
                left.neighbourPoints[7] = leftPos;
                left.neighbourPoints[0] = leftPos;

                MapPoint right = mapPoints[mapSize.x - 1, y];
                Vector2 rightPos = right.transform.position;
                right.neighbourPoints[4] = rightPos;
                right.neighbourPoints[3] = rightPos;
                right.neighbourPoints[2] = rightPos;
            }
        }


        private Vector2 IntermediatePoint(params MapPoint[] points) {
            Vector3 sum = Vector2.zero;
            foreach(MapPoint point in points) {
                sum += point.transform.position;
            }

            return new Vector2(sum.x, sum.y) / points.Length;
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
