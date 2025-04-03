namespace Character.Creator
{
    /// <summary>
    /// Representation of save data that is entirely serializable
    /// This can be converted to and from <see cref="ICustomizationData"/>
    /// </summary>
    [System.Serializable]
    public sealed class CustomizationSavedData
    {
        public string Name;

        public CustomizationSavedData(ICustomizationData data)
        {
            Name = data.Name;
        }
    }

}