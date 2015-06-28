# Version 3.1.3.0

EveAI.Core :

- New static data (Carnyx_1.0_113321).

# Version 3.1.2.0

EveAI.Core :

- New static data (Scylla_1.0_111482).

# Version 3.1.1.0

EveAI.Core :

- New static data (Proteus_1.0_109795).
- Fix a bug in the DataReader (null exception), thx Mr_Mint for the bug fix.

# Version 3.1.0.0

EveAI.Live :

- Add multiCharacterTraining support for AccountStatus.
- CharacterSheet : add homeStationID, homeStation, freeSkillPoints, LastRespecDate, LastTimedRespec, FreeRespecs, CloneJumpDate, List<JumpClone> JumpClones, List<Implant> Implants, JumpActivation, JumpFatigue, JumpLastUpdate, RemoteStationDate. Remove CloneName and CloneSkillpoints : no longer usefull with Rhea. Remove the Augmentator part from the API, no longer exist. Will need some fix to calculate them based on the implants.
- IndustryJob : add the SuccessfulRuns (for invention). Change the Probability type from int to double.

EveAI.Core :

- New static data (Rhea_1.0_109013).
- Fix a bug in the DataReader (null exception).

# Version 3.0.0.0

EveAI.Live :

- An applicationName parameter is now mandatory to create the EveApi object. The applicationName is used in the userAgent
- You no longer need to use "advanced mode" when you want to call an API function with parameter. Now you can call functions from the EveApi object methods
- A couple of functions/classes has been renamed. If your code is no longer valide don't worry, you just need to find the new name (sorry the renaming are not in the changelog)
- A lot, lot, lot of background changes to have a better code
- New static data (Hyperion_1.0_101505)
- Implementation of the OwnerID API : GetOwnerInformations
- Implementation of the CharacterAffiliation API : GetCharacterAffiliation
- Implementation of the PI API endpoints : GetCharacterPlanetaryColonies, GetCharacterPlanetaryLinks, GetCharacterPlanetaryPins, GetCharacterPlanetaryRoutes
- Implementation of the Customs Offices API : GetCorporationCustomsOffices
- Implementation of the Corporation Facilities API : GetCorporationFacilities
- Implementation of the Character and Corporation IndustryJobsHistoryApi (same as IndustryJobsApi but with historic)
- Implementation of the Character and Corporation BlueprintsApi
- Changes on Character and Corporation IndustryJobsApi to match CCP changes
- New informations for GetAccountCharacters : allianceID, allianceName, factionID, factionName, faction
- New information for GetCharacterSheet : factionID, faction
- New information for GetCharacterMailMessagesHeader : senderName
- New informations for GetCharacterInfo : factionID, factionName, faction and corporationName for employment history
- New information for GetCorporationSheet : factionName
- New informations for GetMapFactionalOccupancy : victoryPoints, victoryPointThreshold
- New documentation files

EveAI.Core :

- Remove ramAssemblyLines & ramTypeRequirements static data importation.
- Add BaseCostMultiplier to AssemblyLineType.
- Add FactionID to ProductType.
- Add DuplicatingTime, ReverseEngineeringTime and InventionTime to BlueprintType.


# Version 2.6.7.0


- New static data (Rubicon_1.0.4_93577)
- Complete static data : http://alienswarm.csnu.org/EveAI.Data.zip (temporary link)
- Deprecated the GetCertificateTree 


# Version 2.6.6.0


- New static data (Odyssey_1.1_91288)
- Complete static data : http://dl.eve-files.com/media/1309/EveAI.Data.zip
- Implementation of the 2 new API with Odyssey 1.0.1 : char/KillMails.xml.aspx and corp/KillMails.xml.aspx
- Add the typeIDs for a couple of API (see https://forums.eveonline.com/default.aspx?g=posts&t=252021)
- Add the factionID (new in Odyssey 1.0.1) to the CorporationSheetApi 


# Version 2.6.5.0


- New static data (Odyssey_1.0_89097)
- Complete static data : http://dl.eve-files.com/media/1306/EveAI.Data.zip


# Version 2.6.4.0


- Add the MailBodiesApi


# Version 2.6.3.0


- New static data (Retribution_1.1_84566)
- Complete static data : http://dl.eve-files.com/media/1302/EveAI.Data.zip
- Fix a 500 internal server error with the eve-central API
- Remove the obsolete tag for the function which call the standing API : http://wiki.eveonline.com/en/wiki/EVE_API_Character_NPC_Standings


# Version 2.6.2.0


- New static data (Retribution_1.0.7_463858)
- Fix for the PI schematics bug (thx to Fearitude)
- Obsolete tag for the function which call the standing API : http://wiki.eveonline.com/en/wiki/EVE_API_Character_NPC_Standings


# Version 2.6.1.0


- New static data (Retribution_1.0)
- There is a bug on the CCP export in the invMarketGroups table with the parentGroupID (some items have a parentID that is not in the table). So if you use this to get market tree, you may have some bug. 


# Version 2.6.0.0


- New static data (Inferno_1.2_76477)
- Beta parser for EMDR (see #120) 


# Version 2.5.0.0


- New static data (Inferno_1.0_70633)
- Fix GetIdForObject always return -1
- Remove App.config for the cache folder, use EveApiBase.Configuration.BaseDirectory
- The two assemblies are now COM Visible
- Add the queuePosition information for the CharacterSkillQueueApi
- Add the null value for queuePosition in the SkillInTrainingApi 


# Version 2.4.0.1


- New static data (Escalation_1.0_67593)
- Add the CharacterLocationsApi, the CorporationLocationsApi and the TypeNameApi
- Update the MemberTrackingApi so you can use (or not) the advanced mode (thanks to Gorn Che)
- Add an App.config file which allow to change the cache folder (this file is optionnal, you can delete it)
- Add the getCharacterNameLookup and the getCharacterIDLookup function
- WalletJournalApi: fix an infnite loop (thanks to Almeiti du Larr) 


# Version 2.3.1.0


- Fix the rowCount and fromID parameter for WalletTransactionApi
- Fix an issue with the WalletJournalApi 


# Version 2.3.0.0


- New static data (Crucible_1.1_334471) 

# Version 2.2.0.1


- Fix issues with the fromID and the rowCount attributes for the WalletJournalApi
- Add the attribute beforeKillID on the KillLogApi 


# Version 2.2.0.0


- New percentile attribute on the EveCentralMarketApi GetProductStatistics method
- Removal of the EveMetricsMarketApi
- New method for the EveCentralMarketApi : GetProductOrders (quicklook)
- Update for the CorporationSheetApi : now allows request without AuthentificationData 


# Version 2.1.0.1


- Fix an issue in SkillTree (impact on SkillInTraining) 


# Version 2.1.0


- News statics datas for Crucible_1.0_57975 


# Version 2.0.6


- Change the .net framework to 2.0 


# Version 2.0.5


- Implementation of Contracts API for character and corporation.
- Implementation of ContractItems API for character and corporation.
- Implementation of ContractBids API for character and corporation.
- Items in the AssetList now include a rawQuantity.
- MarketOrders now includes "an optional "orderID" parameter that can be provided to fetch any order belonging to your character/corporation".
- CharacterInfo now includes employment history. 


# Version 2.0.4


- Fix the API Key Info API 


# Version 2.0.1


- Implementation of NotificationTexts API
- Signature of the assemblies with a strong name key 


# Version 2.0


- Completed implementation of the news API Keys (the old ones are no longer supported)
- Update of the cache system
- Completed implementation of ApiKeyInfo API
- CharacterID is not mandatory for all API, check http://wiki.eveonline.com/en/wiki/EVE_API_Functions
- Fix the GetCorporationMemberSecurity 