using Scripts.Map;
using Scripts.Map.Room;
using System.Linq;
using UnityEngine;

namespace Scripts.UI
{
    public class OpenGameMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _buildPanel;

        [SerializeField]
        private Material _materials;

        private void Start()
        {
            _buildPanel.SetActive(false);
        }

        public void ToggleBuildPanel()
        {
            _buildPanel.SetActive(!_buildPanel.activeInHierarchy);
        }

        private RoomType? _currentSelection;
        public void SetCurrentBuild(int type)
        {
            var rType = (RoomType)type;
            _currentSelection = rType == _currentSelection ? (RoomType?)null : rType;
        }

        private GenericRoom clicked = null;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _currentSelection != null)
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var pos2d = (Vector2)pos;
                clicked = MapManager.S.MapRooms.FirstOrDefault((x) =>
                {
                    var xSize = x.Size.x / 2f;
                    return pos2d.x > x.GameObject.transform.position.x - xSize && pos2d.x < x.GameObject.transform.position.x + xSize
                    && pos2d.y > x.GameObject.transform.position.y && pos2d.y < x.GameObject.transform.position.y + x.Size.y;
                }) as GenericRoom;
            }
            if (Input.GetMouseButtonUp(0) && clicked != null && clicked.IsBuilt)
            {
                clicked = null;
            }
        }
    }
}