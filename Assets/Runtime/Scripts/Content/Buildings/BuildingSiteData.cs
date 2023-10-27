using App.Architecture.AppData;
using App.Content.Entities;
using UnityEngine;

namespace App.Content.Buildings
{
    public class BuildingSiteData
    {
        [SerializeField] private InteractionRequirementsComp _buildRequirements;
        [SerializeField] private InteractableComp _interactableComp;
        [SerializeField] private Transform _requirementsPanelTarget;
        [SerializeField] private AScriptableFactory _buildingFactory;
        [SerializeField] private Transform _builderTransform;
    }
}