using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SideProject.MapEdit
{
    [CustomEditor(typeof(MapManager))]
    public sealed class MapEditor : Editor
    {
        private MapManager mapManager;
        private VisualElement uiContainer;
        private ObjectField pointObjectField;
        private TextField mapNameField;
        private Button exportButton;
        private Button importButton;
        private string mapName;

        /// <summary>
        /// Creates the custom inspector GUI components.
        /// </summary>
        public override VisualElement CreateInspectorGUI ()
        {
            uiContainer = new VisualElement();

            CreateMapNameField();
            CreatePointObjectField();
            CreateExportButton();
            CreateImportButton();

            mapManager = (MapManager)target;

            return uiContainer;
        }

        /// <summary>
        /// Adds a text field for entering the map name to the UI.
        /// </summary>
        private void CreateMapNameField ()
        {
            mapNameField = new TextField("Map name: ");
            uiContainer.Add(mapNameField);
        }

        /// <summary>
        /// Adds an object field for selecting a GameObject as point to the UI.
        /// </summary>
        private void CreatePointObjectField ()
        {
            pointObjectField = new ObjectField("Point object here:");
            pointObjectField.objectType = typeof(GameObject);
            uiContainer.Add(pointObjectField);
        }

        /// <summary>
        /// Adds an export button to the UI.
        /// </summary>
        private void CreateExportButton ()
        {
            exportButton = new Button(ExportMap);
            exportButton.text = "Export Map to JSON";
            uiContainer.Add(exportButton);
        }

        /// <summary>
        /// Adds an import button to the UI.
        /// </summary>
        private void CreateImportButton ()
        {
            importButton = new Button(ImportMap);
            importButton.text = "Import from JSON";
            uiContainer.Add(importButton);
        }

        /// <summary>
        /// Handles the export map button click.
        /// </summary>
        private void ExportMap ()
        {
            ValidateMapName();
            mapManager.ExportMapToJson(mapName);
        }

        /// <summary>
        /// Handles the import map button click.
        /// </summary>
        private void ImportMap ()
        {
            ValidateMapName();
            GameObject obj = pointObjectField.value as GameObject;
            mapManager.ImportMapFromJson(mapName, obj);
        }

        /// <summary>
        /// Validates and sets the map name, providing a default if none is provided.
        /// </summary>
        private void ValidateMapName ()
        {
            mapName = mapNameField.value;
            if (string.IsNullOrEmpty(mapName))
            {
                Debug.LogWarning("Map name is empty. Using GameObject name instead.");
                mapName = mapManager.name;
                mapNameField.value = mapName;
            }
        }
    }

}

