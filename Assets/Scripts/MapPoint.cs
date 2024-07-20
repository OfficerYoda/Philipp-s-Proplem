using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace de.officeryoda {
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapPoint : MonoBehaviour {

        public Color color;
        private SpriteRenderer rend;

        /*
         * 6 5 4
         * 7 x 3
         * 0 1 2
         * for neighbours and neighbour Points
         */
        public MapPoint[] neighbours = new MapPoint[8];
        public Vector2[] neighbourPoints = new Vector2[8];
        public bool drawGizmos;

        public List<Vector3> borderPoints = new();


        private void Start() {
            rend = GetComponent<SpriteRenderer>();
            //Color[] colors = { Color.red, Color.green, Color.blue, Color.cyan };
            //color = colors[Random.Range(0, 4)];
            //rend.color = color;
        }

        [EditorCools.Button]
        private void FindBoundary() {
            HashSet<Vector3> boundPoints = new();
            HashSet<MapPoint> seenPoints = new();
            ContributeToBoundary(ref boundPoints, ref seenPoints);
            borderPoints = boundPoints.ToList();
        }

        // works like flood fill
        private void ContributeToBoundary(ref HashSet<Vector3> borderPoints, ref HashSet<MapPoint> seenPoints) {
            // Finding possible edge points
            List<Vector3> borderCandidates = new();
            for(int i = 0; i < neighbours.Length; i++) {
                MapPoint neighbour = neighbours[i];
                if(neighbour.color == this.color && neighbour != this) continue;
                borderCandidates.Add(neighbourPoints[i]);
            }

            borderPoints.UnionWith(borderCandidates);
            seenPoints.Add(this);

            // The flood fill part
            foreach(MapPoint neighbour in neighbours) {
                if(neighbour.color != this.color) continue; // different team
                if(seenPoints.Contains(neighbour)) continue; // already seen

                neighbour.ContributeToBoundary(ref borderPoints, ref seenPoints);
            }
        }

        private void OnValidate() {
            if(!rend) {
                rend = GetComponent<SpriteRenderer>();
            }
            rend.color = color;
        }

        private void OnDrawGizmos() {
            Gizmos.color = color;
            for(int i = 0; i < borderPoints.Count; i++) {
                Vector3 point = borderPoints[i];
                Gizmos.DrawSphere(point, 0.05f);
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = color;
            foreach(Vector2 point in neighbourPoints) {
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
    }
}
