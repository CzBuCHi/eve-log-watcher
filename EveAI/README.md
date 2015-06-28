# EveAI (EVE-Online API/class library for .Net/ C#/ VB.Net)

## What is EveAI ?

EveAI is a .NET library that give you access to all of the Eve Online `API functions` and the Eve Online `static data`.
EveAI is completely self-contained and does not need any database or other libraries except the .NET/Mono framework. EveAI is completely platform independent and has been tested on Windows/.Net and various flavors of Linux using Mono.

`You don't have to worry about the return format (XML, etc), the cache or other things.
Just call a function and you'll get the result with nice class and attributs.`

**The main advantage of EveAI : you have an automatic reference resolution across the API data and the static data (provided by CCP in the SDE).
Example : you call the getCharacterAssets function which return a List of Asset objects.
But what you want to know is the volume of the items, which is not in the API.
With EveAI you have a Type attribut in every Asset object, which contain the data from the SDE. You can find the volume, baseprice, capacity, mass, needed materials, etc.
This is for assets, but there is also solar system, blueprints, etc.**

## Detail of what can do EveAI

- EveAI.Live class library
	- Easy-to-use interface to the Eve API real-time data retrieval mechanism and other Eve Online 3rd party API
    	- Automatic reference resolution even across other live API calls (example : if the API return allianceID, EveAI will automaticly call the AllianceAPI to create an alliance object)
    	- Only needs one line of code to set up
	- Alternative complex interface for more demanding needs
    	- Override caching or implement your own (EveAI have a default cache to avoid useless API call)
	- Structured object-oriented design that is easily expanded for new APIs
	- Structured object-oriented objects that can be used in any application
	- Support for all published API v2 accessors
	- Full automatic support for caching requirements
	- Support for timezone-compensation

- EveAI.Core class library
	- Fully self-contained static Eve Data
	- Does not require *any* database or other components
	- Used to supply static data to EveAI.Live

Some data are not included by default to EveAI.Core because they are to big :
	- Moons/asteriod fields
	- Celestial statistics
If you want to include these data, download the complete static Eve Oline data files EveAI.Data.zip and put it in the same folder that the dll.

## How to use

First download EveAI and add references in your project.

Then you first need to create an EveApi object :

	EveApi api = new EveApi("your application name", keyId, "vCode", characterID);

The application name parameter is used to create a personnalized user agent (mandatory by CCP).
The keyID and vcode are the classic requirement for every applications with API call.
The characterID is optionnal, it depend of how you create your API key (if you used "account" you need the characterID).

You could also proceed like this (same result) :

    AuthenticationData auth = new AuthenticationData()
    {
        KeyID = keyID,
        VCode = "vcode",
        CharacterID = characterID
    };

	EveApi api = new EveApi("your application name");
	api.Authentication = auth;

Now you just have to call whatever you want, for example :
	List<Blueprint> blueprints = api.GetCharacterBlueprints();

Or with a parameter when it's needed :
	CorporationStarbase starbase = api.GetCorporationStarbaseDetail(starbaseID);

You can also just use the static data part :
    EveAI.DataCore dc = new DataCore();
    EveAI.DataReader dr = new StaticDataDataReader(dc, "Path to the folder containing EveAI.Data.zip");

And for example :
    dr.Read(DataTypes.Celestials);

## Available functions

### Account
- GetAccountCharacters
- GetAccountStatus
- GetAllianceData
- GetApiKeyInfo

### Character
- GetCharacterAccountBalance
- GetCharacterAffiliation
- GetCharacterAssets
- GetCharacterBlueprints
- GetCharacterCalendarEventAttendees
- GetCharacterCalendarUpcomingEvents
- GetCharacterContactList
- GetCharacterContactNotifications
- GetCharacterContract
- GetCharacterContractBids
- GetCharacterContractItems
- GetCharacterContracts
- GetCharacterFactionStats
- GetCharacterIndustryJobs
- GetCharacterIndustryJobsHistory
- GetCharacterInfo
- GetCharacterKillLog
- GetCharacterKillLog(long beforeKillID)
- GetCharacterKillMails
- GetCharacterKillMails(int rowCount, long fromID)
- GetCharacterLocations
- GetCharacterMailBodies
- GetCharacterMailingLists
- GetCharacterMailMessagesHeader
- GetCharacterMarketOrder
- GetCharacterMarketOrders
- GetCharacterMedals
- GetCharacterNotificationsHeader
- GetCharacterNotificationTexts
- GetCharacterPlanetaryColonies
- GetCharacterPlanetaryLinks
- GetCharacterPlanetaryPins
- GetCharacterPlanetaryRoutes
- GetCharacterResearch
- GetCharacterSheet
- GetCharacterSkillInTraining
- GetCharacterSkillQueue
- GetCharacterStandings
- GetCharacterWalletJournal
- GetCharacterWalletJournal(int rowCount, long fromID)
- GetCharacterWalletTransactions
- GetCharacterWalletTransactions(int rowCount,  long fromID)

### Corporation
- GetCorporationAccountBalance
- GetCorporationAssets
- GetCorporationBlueprints
- GetCorporationContactList
- GetCorporationContainerLog
- GetCorporationContract
- GetCorporationContractBids
- GetCorporationContractItems
- GetCorporationContracts
- GetCorporationCustomsOffices
- GetCorporationFacilities
- GetCorporationFactionStats
- GetCorporationIndustryJobs
- GetCorporationIndustryJobsHistory
- GetCorporationKillLog
- GetCorporationKillLog(long beforeKillID)
- GetCorporationKillMails
- GetCorporationKillMails(int rowCount, Nullable<long> fromID)
- GetCorporationLocations
- GetCorporationMarketOrder
- GetCorporationMarketOrders
- GetCorporationMedals
- GetCorporationMemberMedals
- GetCorporationMemberSecurity
- GetCorporationMemberSecurityLog
- GetCorporationMemberTracking
- GetCorporationMemberTracking(bool extended)
- GetCorporationShareHolders
- GetCorporationSheet
- GetCorporationSheet(long corporationID)
- GetCorporationStandings
- GetCorporationStarbaseDetail
- GetCorporationStarbaseList
- GetCorporationTitles
- GetCorporationWalletJournal(int accountKey)
- GetCorporationWalletJournal(int accountKey, int rowCount, long fromID)
- GetCorporationWalletTransactions(int accountKey)
- GetCorporationWalletTransactions(int accountKey, int rowCount, long fromID)

### Eve (generic, map)
- ConvertIDsToNames
- ConvertNamesToIDs
- DownloadAllianceImage
- DownloadAllianceImageData
- DownloadCharacterImage
- DownloadCharacterImageData
- DownloadCorporationImage
- DownloadCorporationImageData
- GetConquerableStations
- GetErrorList
- GetFactionalTop100
- GetFactionalWarStatistics
- GetMapFactionalOccupancy
- GetMapJumps
- GetMapKillEntries
- GetMapSovereignty
- GetOutpostList
- GetOutpostServices
- GetOwnerInformations
- GetServerStatus
- GetSkillTree
- GetTypeNames
- GetWalletTransferTypes
