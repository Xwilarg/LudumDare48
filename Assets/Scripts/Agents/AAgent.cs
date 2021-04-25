using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Map;
using Scripts.Map.Room;
using Scripts.Resources;
using Scripts.Events;

namespace Scripts.Agents
{
    public abstract class AAgent : MonoBehaviour
    {

        protected int _id;

        protected Dictionary<ResourceType, int> _inventory;

        public static int IdRef = 0;
        protected ARoom _currentRoom;

        private string _childClassName;

        protected List<(List<ARoom> path, Action action)> _actions;

        public enum Action
        {
            Idle,
            Move,
            TakeRessource,
            DropRessource,
            GenerateBlueprint,
        }

        public AAgent(string childClassName)
        {
            _childClassName = childClassName;
            _id = ++IdRef;
            _inventory = new Dictionary<ResourceType, int>();

        }
        public abstract void OnEventReceived(Events.Event e, object o);
        protected abstract void DoStartAction();

        public void Start()
        {
            name = _childClassName + " " + _id;
            _actions = new List<(List<ARoom> path, Action action)>();
            WhereAmI();
            DoStartAction();
        }

        private void WhereAmI()
        {
            if (_currentRoom == null)
            {
                Debug.Log("  Checking for room");
                Debug.Log("  My pos : " + transform.position.x + ", " + transform.position.y);
                foreach (var room in MapManager.S.MapRooms)
                {
                    Debug.Log("    Room pos :");
                    Debug.Log("      (" + room.Position.x + ", " + room.Position.y + ") (" + (room.Position.x + room.Size.x) + ", " + (room.Position.y + room.Size.y) + ")");

                    if (transform.position.x >= room.Position.x && transform.position.x <= room.Position.x + room.Size.x &&
                       -transform.position.y >= room.Position.y && -transform.position.y <= room.Position.y + room.Size.y
                    )
                    {
                        _currentRoom = room;
                        break;
                    }
                }
                // if _currentRoom is still null ... we are in the water !
                if (_currentRoom == null)
                {
                    Debug.Log("  Still no room => aborting");
                }
            }
        }

        public void MoveTo(ARoom room)
        {
            _currentRoom = room;
            transform.position = new Vector3(
                _currentRoom.Position.x + 0.5f,
                -_currentRoom.Position.y + 0.1f,
                0f
            );
            DoNextAction();
        }

        public Action MoveOrGetAction()
        {
            if (_actions.Count > 0)
            {
                if (_actions[0].path.Count > 0)
                {
                    MoveTo(_actions[0].path[0]);
                    _actions[0].path.RemoveAt(0);
                    return Action.Move;
                }
                else
                {
                    Action action = _actions[0].action;
                    _actions.RemoveAt(0);
                    return action;
                }
            }
            return Action.Idle;
        }



        public abstract void DoSpecialAction(Action action);
        public abstract void ChooseAction();

        public void DoNextAction()
        {
            Action action = MoveOrGetAction();
            if (action != Action.Move && action != Action.Idle)
            {
                DoSpecialAction(action);
            }
            if (action == Action.Idle)
            {
                //ChooseAction();
                return;
            }
            DoNextAction();
        }

    }

}
