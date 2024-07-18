using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace de.officeryoda {
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapPoint : MonoBehaviour {

        public Color color;
        private SpriteRenderer rend;

        public List<MapPoint> neighbours = new();
        public bool drawGizmos;

        public List<Vector3> borderPoints = new();


        private void Start() {
            rend = GetComponent<SpriteRenderer>();
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
            foreach(MapPoint neighbour in neighbours) {
                if(neighbour.color == this.color && neighbour != this) continue;
                Vector2 center = (transform.position + neighbour.transform.position) / 2f;
                borderCandidates.Add(center);
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
            //foreach(Vector3 point in borderCandidate) {
            //    Gizmos.DrawSphere(point, 0.05f);
            //}
            foreach(Vector3 point in borderPoints) {
                Gizmos.DrawSphere(point, 0.05f);
            }

            //if(!drawGizmos) return;

            //Gizmos.color = color;
            //foreach(MapPoint neighbour in neighbours) {
            //    Vector2 center = (transform.position + neighbour.transform.position) / 2f;
            //    Gizmos.DrawSphere(center, 0.05f);
            //}
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = color;
            foreach(MapPoint neighbour in neighbours) {
                Vector2 center = (transform.position + neighbour.transform.position) / 2f;
                Gizmos.DrawSphere(center, 0.05f);
            }
        }
    }
}
