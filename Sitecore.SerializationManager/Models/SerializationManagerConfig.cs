using System;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Extensions;

namespace Sitecore.SerializationManager.Models
{
    public class SerializationManagerConfig
    {
        public virtual string CurrentUser { get; set; }
        public virtual string CurrentDatabase { get; set; }
        public virtual ItemVersion DefaultVersion { get; set; }

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