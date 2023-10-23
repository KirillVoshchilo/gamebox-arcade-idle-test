using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldCanvasStorage : MonoBehaviour
{
    [SerializeField] private InteractIcon _interactIcon;
    [SerializeField] private RequirementsPanel _requirementsPanel;

    public InteractIcon InteractIcon
        => _interactIcon;
    public RequirementsPanel RequirementsPanel 
        => _requirementsPanel; 
}
