using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TTGame
{
    public class WayPointView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private LineRenderer lineRendererPrefab;
        private List<LineRenderer> lineRenderers = new();
        public List<WayPointView> ways;
        [SerializeField] private ParticleSystem highlight;
        [HideInInspector] public List<WayPointView> possiblePoints;
        public EventStream clickStream = new();
        public WayPointModel model = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            clickStream.Invoke();
        }

        public void DrawWays(Material mat, bool useHightlight = false)
        {
            for (int i = 0; i < possiblePoints.Count; i++)
            {
                lineRenderers[i].material = mat;
                lineRenderers[i].SetPosition(0, lineRenderers[i].transform.position.WithY(lineRenderers[i].transform.position.y - 0.2f));
                lineRenderers[i].SetPosition(1, possiblePoints[i].transform.position.WithY(possiblePoints[i].transform.position.y - 0.2f));
                possiblePoints[i].SetHightLight(useHightlight);
            }
        }
        public void HighlightAllPatchs(Material highlight, int sortOrder, bool useHightlight = false)
        {
            lineRenderers.ForEach(lr =>
            {
                // lr.material = highlight;
                lr.sortingOrder = sortOrder;
            });
            DrawWays(highlight, useHightlight);
        }
        public void SetHightLight(bool value)
        {
            if (value) highlight.Play();
            else highlight.Stop();
        }
        public void Init()
        {
            ways.ForEach(w =>
            {
                w.AddPossibleWay(this);
                AddPossibleWay(w);
            });
            model.possiblePoints = possiblePoints.Select(pp => pp.model).ToList();
        }
        public void AddPossibleWay(WayPointView wayPoint)
        {
            if (possiblePoints.Contains(wayPoint)) return;
            possiblePoints.Add(wayPoint);
            var renderer = Instantiate(lineRendererPrefab, transform);
            lineRenderers.Add(renderer);
            renderer.transform.position = transform.position;
        }
    }
}
