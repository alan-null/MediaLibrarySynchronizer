using System;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Extensions;

namespace Sitecore.SerializationManager.Models
{
    public abstract class SerializationManagerConfig
    {
        public abstract string CurrentUser { get; }
        public abstract string CurrentDatabase { get; }
        public abstract ItemVersion DefaultVersion { get; }

        public SyncVersion BuildSyncVersion()
        {
            SyncVersion version = new SyncVersion
            {
                Language = DefaultVersion.Language,
                Version = DefaultVersion.Version,
                Revision = Guid.NewGuid().ToString()
            };
            string date = DateUtil.IsoNowWithTicks;
            version.AddField(StatisticsTemplateFields.Created, date);
            version.AddField(StatisticsTemplateFields.CreatedBy, CurrentUser);
            version.AddField(StatisticsTemplateFields.Revision, Guid.NewGuid().ToString());
            version.AddField(StatisticsTemplateFields.Updated, date);
            version.AddField(StatisticsTemplateFields.UpdatedBy, CurrentUser);
            return version;
        }
    }
}