using System;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class TitleMainManager : MonoBehaviour
    {
        public static TitleMainManager Instance;

        private Rigidbody _selectionsRigidbody;
        private GameObject _selectionsContainer;
        private Selection[] _selections;
        private int _currentSelectionIndex = 0;
        private float _nextPositionY;
        public int _moveDirection = -1;
        private float _moveSpeed = 5.75f;

        public int CurrentSelectionIndex()
        {
            return _currentSelectionIndex;
        }

        public Selection CurrentSelection => _selections[CurrentSelectionIndex()];

        public int SelectionsLength
        {
            get => _selections.Length;
        }

        public string Selected { get; private set; }

        private bool IsChanging { get; set; }

        // runs initially and when scene is loaded
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            if (!IsChanging) return;

            var selectionContainerPos = _selectionsContainer.transform.position;
            var step = _moveSpeed * Time.deltaTime * _moveDirection;
            var yDiff = _nextPositionY - selectionContainerPos.y;

            if (_moveDirection * yDiff < step)
            {
                // final position
                _selectionsRigidbody.transform.position = new Vector3(
                    selectionContainerPos.x,
                    _nextPositionY,
                    selectionContainerPos.z);
                IsChanging = false;
            }
            else
            {
                _selectionsRigidbody.transform.position = new Vector3(
                    selectionContainerPos.x,
                    selectionContainerPos.y + step,
                    selectionContainerPos.z);
            }
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            // TODO add a root scene manager to handle this functionality
            var parsedScene = Enum.Parse(typeof(Scenes), scene.buildIndex.ToString());
            IsChanging = false;
            switch (parsedScene)
            {
                case Scenes.Title:
                    LoadArtillery();
                    break;
            }
        }

        private void LoadArtillery()
        {
            _selectionsContainer = GameObject.Find("Selection Options");
            _selectionsRigidbody = _selectionsContainer.GetComponent<Rigidbody>();
            _selections = _selectionsContainer.GetComponentsInChildren<Selection>();
            _selectionsContainer.transform.Translate(0, CurrentSelectionYPosition(), 0);
            Selected = CurrentSelection.WeaponPrefab;
            UpdateSelectionsDisplay();
        }

        private void UpdateSelectionsDisplay()
        {
            for (var i = 0; i < SelectionsLength; i++)
            {
                if (i == CurrentSelectionIndex())
                {
                    var s = CurrentSelection;
                    //s.WeaponMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
                    s.UpdateTransparency(1f, true, _moveDirection);
                }
                else if (i == CurrentSelectionIndex() - 1 || i == CurrentSelectionIndex() + 1)
                {
                    var s = _selections[i];
                    //s.WeaponMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
                    s.UpdateTransparency(0.15f, false, _moveDirection);
                }
                else
                {
                    var s = _selections[i];
                    //s.WeaponMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                    s.UpdateTransparency(0f, false, _moveDirection);
                }
            }
        }

        public float CurrentSelectionYPosition()
        {
            return 4 * CurrentSelectionIndex();
        }

        public void ChangeSelection(Boolean up)
        {
            var change = false;
            if (up && CurrentSelectionIndex() > 0)
            {
                change = true;
                _moveDirection = -1;
                _currentSelectionIndex--;
            }
            else if (!up && CurrentSelectionIndex() != SelectionsLength - 1)
            {
                change = true;
                _moveDirection = 1;
                _currentSelectionIndex++;
            }

            if (!change) return;

            Selected = CurrentSelection.WeaponPrefab;
            _nextPositionY = CurrentSelectionYPosition();
            IsChanging = true;
            UpdateSelectionsDisplay();
        }
    }
}
