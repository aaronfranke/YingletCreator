namespace Character.Creator
{
    /// <summary>
    /// Representation of save data that is entirely serializable
    /// </summary>
    [System.Serializable]
    public sealed class SerializableCustomizationData
    {
        public string Name;

        public SerializableCustomizationData() { }
        public SerializableCustomizationData(ObservableCustomizationData data)
        {
            Name = data.Name.Val;
        }
    }
}