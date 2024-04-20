using SideProject.MapEdit;
using SideProject.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SideProject.MapEdit
{
    public class MapManager : MonoBehaviour
    {
        private GameObject pointPrefab;
        private MapItem currentMap;
        private Transform[] childTransforms;
        private string serializedMapData;
        private static readonly string pointNameFormat = "Point_{0}";
        private static readonly string googleScriptUrl = "https://script.google.com/macros/s/AKfycbx9o9ZZ950Nyknu8IGdF2geNVQ1dh_9_7ksAjDE5IMOSsFI1gl5C7QQuCnPPYBfOMqX3Q/exec";

        /// <summary>
        /// Exports the current map to JSON format.
        /// </summary>
        /// <param name="mapName">The name of the map to export.</param>
        public void ExportMapToJson (string mapName)
        {
            SerializeMap(mapName);
            UploadData(serializedMapData);
            Debug.Log("ExportMap ---> " + serializedMapData);
        }

        /// <summary>
        /// Imports a map from JSON format using a specified point prefab.
        /// </summary>
        /// <param name="mapName">The name of the map to import.</param>
        /// <param name="pointPrefab">The prefab used for points within the map.</param>
        public void ImportMapFromJson (string mapName, GameObject pointPrefab)
        {
            if (pointPrefab == null)
            {
                Debug.LogWarning("No default point prefab provided.");
                return;
            }
            this.pointPrefab = pointPrefab;
            PrepareMapForImport(mapName);
            Debug.Log("ImportMap: ---> " + mapName);
            GASManager.Instance.DoGet(googleScriptUrl, mapName, OnJsonDataReceived);
        }

        /// <summary>
        /// Handles the JSON data received for importing a map.
        /// </summary>
        /// <param name="jsonData">The JSON string containing map data.</param>
        private void OnJsonDataReceived (string jsonData)
        {
            DeserializeMap(jsonData);
            if (currentMap == null)
            {
                return;
            }
            InstantiatePoints();
            Debug.Log("ImportMap: ---> " + currentMap);
        }

        /// <summary>
        /// Serializes the current map state to JSON.
        /// </summary>
        /// <param name="mapName">The name of the map to serialize.</param>
        private void SerializeMap (string mapName)
        {
            childTransforms = this.GetComponentsInChildren<Transform>();
            int length = childTransforms.Length;

            currentMap = new MapItem
            {
                name = mapName,
                pointList = new List<PointItem>()
            };

            for (int i = 0; i < length; i++)
            {
                if (childTransforms[i].gameObject == this.gameObject)
                {
                    continue;
                }
                var point = new PointItem
                {
                    id = i,
                    x = childTransforms[i].position.x,
                    y = childTransforms[i].position.y
                };

                currentMap.pointList.Add(point);
            }
            serializedMapData = JsonUtility.ToJson(currentMap);
        }

        /// <summary>
        /// Uploads serialized data to a server.
        /// </summary>
        /// <param name="data">The serialized map data to upload.</param>
        private void UploadData (string data)
        {
            GASManager.Instance.DoPost(googleScriptUrl, data);
        }

        /// <summary>
        /// Prepares the map for importing by clearing existing data.
        /// </summary>
        /// <param name="mapName">The name of the map for which to prepare.</param>
        private void PrepareMapForImport (string mapName)
        {
            this.name = mapName;
            ClearExistingChildren();
        }

        /// <summary>
        /// Clears existing children from the map.
        /// </summary>
        private void ClearExistingChildren ()
        {
            int count = this.transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Transform child = this.transform.GetChild(i);
                if (child.TryGetComponent(out Image image))
                {
                    DestroyImmediate(image);
                }
                DestroyImmediate(child.gameObject);
            }
        }

        /// <summary>
        /// Deserializes JSON string into map data.
        /// </summary>
        /// <param name="jsonString">The JSON string to deserialize.</param>
        private void DeserializeMap (string jsonString)
        {
            try
            {
                currentMap = JsonUtility.FromJson<MapItem>(jsonString);
            }
            catch
            {
                Debug.LogWarning("Failed to deserialize JSON: " + jsonString);
            }
        }

        /// <summary>
        /// Instantiates points on the map based on the deserialized map data.
        /// </summary>
        private void InstantiatePoints ()
        {
            foreach (var point in currentMap.pointList)
            {
                Vector3 position = new Vector3(point.x, point.y);
                GameObject newPoint = Instantiate(pointPrefab, position, Quaternion.identity, this.transform);
                newPoint.name = string.Format(pointNameFormat, point.id);
                newPoint.SetActive(true);
            }
        }
    }
}