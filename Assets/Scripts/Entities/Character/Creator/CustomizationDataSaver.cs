using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationDataSaver
    {
        void SaveCurrent();
        void DeleteCurrent();

    }

    public class CustomizationDataSaver : MonoBehaviour, ICustomizationDataSaver
    {
        const string EXTENSION = ".yingsave";

        private ICharacterCreatorDataRepository _dataRepository;
        private ICustomizationSaveFolderProvider _locationProvider;

        void Awake()
        {
            _dataRepository = this.GetComponent<ICharacterCreatorDataRepository>();
            _locationProvider = this.GetComponent<ICustomizationSaveFolderProvider>();
        }

        public void SaveCurrent()
        {
            var data = _dataRepository.CustomizationData;
            var fileName = SanitizeNameToFilepath(data.Name);
            var pathOnDisk = Path.Combine(_locationProvider.FolderRoot, fileName);
            string json = JsonUtility.ToJson(new CustomizationSavedData(data), true);
            File.WriteAllText(pathOnDisk, json);
        }
        public void DeleteCurrent()
        {
            // TODO
        }

        string SanitizeNameToFilepath(string actualName)
        {
            if (string.IsNullOrWhiteSpace(actualName))
            {
                actualName = "Unnamed";
            }
            var sanitized = Regex.Replace(actualName, "[^a-zA-Z]", "");
            return sanitized + EXTENSION;
        }

    }
}
