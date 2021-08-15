using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TitleMainManager : MonoBehaviour
{
    public static TitleMainManager Instance;
    [SerializeField] public float alphaChangeStep = 0.009f;
    
    [SerializeField] private GameObject selectionsContainer;
    [SerializeField] private Selection[] selections;
    [SerializeField] private int currentSelectionIndex;
    [SerializeField] private string selectedWeapon;
    private Boolean _isChanging;
    [SerializeField] private float _nextPositionY;
    [SerializeField] private int _moveDirection = -1;
    [SerializeField] private float _moveSpeed = 0.025f;
    public string Selected
    {
        get => selectedWeapon;
        set => selectedWeapon = value;
    }

    public bool IsChanging
    {
        get => _isChanging;
        set => _isChanging = value;
    }

    private Rigidbody selectionsRigidbody;

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

        var finalY = Mathf.Round(_nextPositionY);
        var selectionContainerPos = selectionsContainer.transform.position;
        var final = new Vector3(selectionContainerPos.x, _nextPositionY, selectionContainerPos.z);
        
        if (Mathf.Approximately(selectionContainerPos.y, finalY))
        {
            selectionsRigidbody.transform.Translate(0, selectionContainerPos.y - final.y, 0);
            _isChanging = false;
        }
        else
        {
            selectionsRigidbody.MovePosition(final + _moveSpeed * Time.deltaTime * _moveDirection * selectionContainerPos);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO add a root scene manager to handle this functionality
        var parsedScene = Enum.Parse(typeof(Scenes), scene.buildIndex.ToString());
        IsChanging = false;
        switch (parsedScene)
        {
            case Scenes.Title:
                selectionsRigidbody = GameObject.Find("Selection Options").GetComponent<Rigidbody>();
                LoadArtillery();
                break;
        }
    }

    private void LoadArtillery()
    {
        selectionsContainer = GameObject.Find("Selection Options");
        Selection[] selectionsChildren = selectionsContainer.GetComponentsInChildren<Selection>();
        selections = new Selection[selectionsChildren.Length];
        for (int i = 0; i < selectionsChildren.Length; i++)
        {
            selections[i] = selectionsChildren[i];
        }
        selectionsContainer.transform.Translate(0, CurrentSelectionYPosition(), 0);
        Selected = CurrentSelection().GetWeaponPrefab;
        UpdateSelectionsDisplay();
    }

    private void UpdateSelectionsDisplay()
    {
        for (int i = 0; i < selections.Length; i++)
        {
            if (i == currentSelectionIndex)
            {
                var s = CurrentSelection();
                var mr = s.GetBaseGameObject().GetComponent<MeshRenderer>();
                s.UpdateTransparency(1f, true);
                mr.shadowCastingMode = ShadowCastingMode.On;
            }
            else if (i == currentSelectionIndex - 1 || i == currentSelectionIndex + 1)
            {
                var s = selections[i];
                var mr = s.GetBaseGameObject().GetComponent<MeshRenderer>();
                s.UpdateTransparency(0.15f, false);
                mr.shadowCastingMode = ShadowCastingMode.On;
            }
            else
            {
                var s = selections[i];
                var mr = s.GetBaseGameObject().GetComponent<MeshRenderer>();
                s.UpdateTransparency(0f, false);
                mr.shadowCastingMode = ShadowCastingMode.Off;
            }
        }
    }
    
    public float CurrentSelectionYPosition()
    {
        return 4 * currentSelectionIndex;
    }
    
    public int CurrentSelectionIndex()
    {
        return currentSelectionIndex;
    }

    public Selection CurrentSelection()
    {
        return selections[currentSelectionIndex];
    }

    public int SelectionsLength()
    {
        return selections.Length;
    }
    
    public void ChangeSelection(Boolean up)
    {
        var change = false;
        var prevPosY = CurrentSelectionYPosition();
        if (up && currentSelectionIndex > 0)
        {
            change = true;
            _moveDirection = -1;
            currentSelectionIndex--;
        } 
        else if (!up && currentSelectionIndex != selections.Length - 1)
        {
            change = true;
            _moveDirection = 1;
            currentSelectionIndex++;
        }
        
        if (!change) return;
        
        Selected = CurrentSelection().GetWeaponPrefab;
        _nextPositionY = CurrentSelectionYPosition();
        IsChanging = true;
        UpdateSelectionsDisplay();
    }
}
