using System.IO;
using System.Linq;
using UnityEngine;
using ScenePlaymat.Data.Missions;

namespace ScenePlaymat.MonoBehaviours
{
    public class MissionLoader : MonoBehaviour
    {
        public TextAsset missionsAsJson;
        public MissionGroup groupToPopulateFromJson;
        public MissionGroup groupToSaveAsJson;

        /// <summary>
        /// Only used when saving json; for loading, use the TextAsset, missionsAsJson.
        /// Note: this is only for dev/test.
        /// Default value:  = "Assets/ScenePlaymat/Data/Missions/SampleMissions.json";
        /// </summary>
        public string pathToSaveJsonAt;
        
        private void Awake()
        {
            Debug.Assert(groupToPopulateFromJson != null, $"Expected {nameof(groupToPopulateFromJson)} != null)");
            //SaveMissionsAsJson(groupToSaveAsJson, pathToSaveJsonAt);
            PopulateMissionsFromJson(groupToPopulateFromJson, missionsAsJson.text);
        }

        /// <summary>
        /// For dev/testing purposes, saves a mission group as a resource.
        /// </summary>
        /// <param name="groupToSave"></param>
        /// <param name="path"></param>
        public void SaveMissionsAsJson(MissionGroup groupToSave, string path)
        {
            // Unity's json utility has some limitations:
            // - Collections and primitives must not be top-level properties; wrap them in another object.
            // - Doesn't seem to support a ScriptableObject within another ScriptableObject; cry.
            //    I made a tiny duplicate class that isn't an SO with similar properties, only used for serialization.
            var missions = groupToSave.missions.Select(mission => new MissionSerializationHelper {data = mission.data}).ToList();
            
            var groupHelper = new MissionGroupSerializationHelper
            {
                displayName =  groupToSave.displayName,
                description = groupToSave.description,
                missions = missions
            };

            var json = JsonUtility.ToJson(groupHelper, true);
            Debug.Log("MissionLoader: json to save: " + json);

            using var filesStream = new FileStream(path, FileMode.Create);
            using var streamWriter = new StreamWriter(filesStream);
            streamWriter.Write(json);
            Debug.Log("MissionLoader: file saved to path: " + path);
        }

        public void PopulateMissionsFromJson(MissionGroup groupToPopulate, string json)
        {
            Debug.Log("MissionLoader: Deserializing json: " + json);
            var missionGroupHelper = JsonUtility.FromJson<MissionGroupSerializationHelper>(json);
            Debug.Log("MissionLoader: MissionGroupHelper.missions.Count " + missionGroupHelper.missions.Count);
            
            groupToPopulate.displayName = missionGroupHelper.displayName;
            groupToPopulate.description = missionGroupHelper.description;
            groupToPopulate.missions.Clear();
            foreach (var otherMission in missionGroupHelper.missions)
            {
                var realMission = ScriptableObject.CreateInstance<Mission>();
                realMission.data = otherMission.data;
                groupToPopulate.missions.Add(realMission);
            }

            Debug.Log("MissionLoader: groupToPopulate.missions.Count: " + groupToPopulate.missions.Count);
            LogPopulatedMissionGroup(groupToPopulate);
        }

        public void LogPopulatedMissionGroup(MissionGroup groupToLog)
        {
            foreach (var mission in groupToLog.missions)
            {
                Debug.Log("MissionLoader: Populated mission: " + mission.data.displayName);
            }
        }
    }
}