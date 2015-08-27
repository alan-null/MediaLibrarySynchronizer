namespace MediaLibrarySynchronizer.ConfigurationProviders
{
    interface IConfigProvider<T>
    {
        ConfigurationWrapper<T> GetConfig();
    }
}
