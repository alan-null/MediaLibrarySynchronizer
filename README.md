# Media Library Synchronizer
Console application for media library serialization data synchronization. 


## Why?
What if I want to modify serialized media library items but I don't have Sitecore instance?
I cannot. Out of the box I am only able to attach media to the Sitecore item and then update serialization file via Sitecore.

This case can be easily covered by MediaLibrarySynchronizer.   


## How it works?
Serialization data will be modified or created depending on config file. Application acts like a Sitecore but the power of MediaLibrarySynchronizer is that it doesn't require IIS and SQL. All you need is just a one exe file with few dlls.

Application will scan source folder and then recreate whole structure using Sitecore items, serialized into **.item* files, so you can easily update tree in Sitecore, to fetch media into database later.         

## Features
* serialization without Sitecore instance
* file system to serialization data synchronization
* can be used like Sitecore *upload* folder, but due to its most important job I would call it *serialize* instead, because it operates with **.items* not a database

## How use it?
**1**  Download MediaLibrarySynchronizer and build it

**2**  Copy *config.json.example* file and change the name to *config.json*

**3**  Update json file with your environment configuration

**SerializationManagerConfig**      
```json
    "SerializationManagerConfig": {
        "CurrentUser": "sitecore/admin",
        "DefaultVersion": {
            "Language": "en",
            "Version": "1"
        }
    },
```
CurrentUser - *Updated serialization items will be updated with a given user,*

DefaultVersion - *Updated serialization items will be updated with a given default version*

**SynchronizationConfig**   

Synchronization configurations table. 
```json
    "SynchronizationConfig": [
        {
            "SourcePath": "C:/REPOSITORIES/Example/UI/themes/theme1",
            "Destination": {
                "DataFolder": "C:/WWW/Example/Data",
                "Database": "master",
                "SitecorePath": "sitecore/media library/themes/theme1"
            }
        }
    ]
```
SourcePath - *folder location where source media files are located (regular files),*

DataFolder - *Sitecore data folder where serialization node exists,*

Database - *destination database for serialization items,*

SitecorePath - *destination location in media library*


