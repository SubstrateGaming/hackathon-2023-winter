﻿using Substrate.Hexalem.Engine;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class GridManager : Singleton<GridManager>
    {
        public delegate void TileClickHandler(GameObject tileObject, int index);

        public event TileClickHandler OnGridTileClicked;

        [SerializeField]
        public GameObject PlayerGrid;

        [SerializeField]
        private GameObject _emptyTile;

        [SerializeField]
        private GameObject _homeTile;

        [SerializeField]
        private GameObject _grassTile;

        [SerializeField]
        private GameObject _waterTile;

        [SerializeField]
        private GameObject _mountainTile;

        [SerializeField]
        private GameObject _treesTile;

        [SerializeField]
        private GameObject _desertTile;

        [SerializeField]
        private GameObject _caveTile;

        private Vector2 touchStart;

        private Vector2 touchEnd;

        private bool isSwiping;

        public float swipeThreshold = 50f; // Minimum distance for a swipe

        public float cameraMoveSpeed = 1f; // Speed of camera movement

        private bool isPointerOverUI = false;

        private VisualElement _root;

        private void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        public void RegisterBottomBound()
        {
            _root.Q("BottomBound").RegisterCallback<PointerEnterEvent>(e => isPointerOverUI = true);
            _root.Q("FloatBody").RegisterCallback<PointerEnterEvent>(e => isPointerOverUI = false);
        }

        private void Update()
        {

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStart = touch.position;
                        isSwiping = false;
                        break;

                    case TouchPhase.Moved:
                        touchEnd = touch.position;
                        if (!isSwiping && !isPointerOverUI && Vector2.Distance(touchStart, touchEnd) >= swipeThreshold)
                        {
                            ProcessSwipe(touchEnd.x - touchStart.x, touchEnd.y - touchStart.y);
                            isSwiping = true;
                        }
                        break;

                    case TouchPhase.Ended:
                        if (!isSwiping)
                        {
                            ProcessTap(touch.position);
                        }
                        break;
                }
            }
        }

        private void ProcessSwipe(float xDist, float yDist)
        {
            if (Mathf.Abs(xDist) > Mathf.Abs(yDist))
            {
                MoveCamera(xDist > 0 ? Vector3.left : Vector3.right);
            }
            else
            {
                MoveCamera(yDist > 0 ? Vector3.down : Vector3.up);
            }
        }

        private void MoveCamera(Vector3 direction)
        {
            Camera.main.transform.Translate(direction * 1);
        }

        private void ProcessTap(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.parent != null && hit.transform.parent.name.StartsWith('t'))
            {
                var tileObject = hit.transform.gameObject;
                var index = int.Parse(tileObject.transform.parent.name[1..]);
                Debug.Log($"Tapped on {tileObject.name} [{index}]");
                OnGridTileClicked?.Invoke(tileObject, index);
            }
        }

        public void CreateGrid(HexaBoard hexaBoard)
        {
            for (int i = 0; i < hexaBoard.Value.Length; i++)
            {
                HexaTile tile = hexaBoard.Value[i];

                var gridParent = PlayerGrid.transform.GetChild(i);

                // remove previous game objects connected to the grid
                foreach (Transform child in gridParent.transform)
                {
                    Destroy(child.gameObject);
                }

                GameObject newTile;
                switch (tile.TileType)
                {
                    case TileType.Empty:
                        newTile = Instantiate(_emptyTile, gridParent);
                        break;

                    case TileType.Home:
                        newTile = Instantiate(_homeTile, gridParent);
                        break;

                    case TileType.Grass:
                        newTile = Instantiate(_grassTile, gridParent);
                        break;

                    case TileType.Water:
                        newTile = Instantiate(_waterTile, gridParent);
                        break;

                    case TileType.Mountain:
                        newTile = Instantiate(_mountainTile, gridParent);
                        break;

                    case TileType.Tree:
                        newTile = Instantiate(_treesTile, gridParent);
                        break;

                    case TileType.Desert:
                        newTile = Instantiate(_desertTile, gridParent);
                        break;

                    case TileType.Cave:
                        newTile = Instantiate(_caveTile, gridParent);
                        break;
                }
            }
        }
    }
}