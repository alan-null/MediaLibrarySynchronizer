namespace Sitecore.SerializationManager.Models
{
    public interface IFieldInfo
    {
        string Name { get; }
        string Key { get; }
        string FieldId { get; }
    }
}