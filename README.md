# Fusion Gear Tracker

Use [Warcraft Logs](https://www.warcraftlogs.com) (WCL) to discover when gear is acquired by players in a guild.

## Prerequisites

- [Download and Install .NET 7](https://dotnet.microsoft.com/download)

### Application settings

Make a `appsettings/appsettings.json` file with the following fields:

```json
{
  "appDataPath": "../../appdata/data.json",
  "baseAddress": "https://www.warcraftlogs.com/api/v2/client",
  "clientId": "{your client id}",
  "clientSecret": "{your client secret}",
  "guildId": 123456,
  "sheetsClientId": "{your google client id}",
  "sheetsClientSecret": "{your google client secret}",
  "sheetsSpreadsheetId": "{your google spreadsheet id}",
  "useReportCache": false,
  "updateGear": true,
  "firstReportDate": "2023-05-01",
  "lastReportDate": "2023-01-01",
  "itemsToTrack": [
    { "id": 45516, "name": "Voldrethar", "slot": "Main Hand", "instanceSize": 25 },
    { "id": 45516, "name": "Voldrethar", "slot": "Off Hand", "instanceSize": 25 },
    { "id": 45536, "name": "Cunning Deception", "slot": "Legs", "instanceSize": 25 },
    { "id": 45931, "name": "Mjolnir Runestone", "slot": "Trinket", "instanceSize": 10 }
  ],
  "playersToTrack": [
    { "name": "Feralbestclass", "raid": "Raid A", "class": "Warrior", "spec": "Fury" },
    { "name": "Blizzhateswarr", "raid": "Raid B", "class": "Warrior", "spec": "Fury" }
  ]
}
```

- **appDataPath**: Where application data will be saved
- **baseAddress**: Should be `"https://www.warcraftlogs.com/api/v2/client"`
- **clientId**: Your WCL API client identifier from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **clientSecret**: Your WCL API client secret from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **guildId**: The guild identifier can be found on a WCL guild reports URL
- **sheetsClientId**: Your Google client identifier
- **sheetsClientSecret**: Your Google client secret
- **useReportCache**: Whether or not to use saved report data instead of pulling from WCL
- **updateGear**: Whether or not to update player gear, which will save WCL API Points if just updating the report cache
- **firstReportDate**: The most recent date from which to pull data
- **lastReportDate**: The oldest date from which to pull data
- **itemsToTrack**: A list of item identifiers found on [Wowhead](https://www.wowhead.com)
  - **id**: The id of the item, found on Wowhead
  - **name**: A name of the item, which doesn't have to match Wowhead
  - **slot**: The equipped slot of the item
  - **instanceSize**: The instance size where the item drops
- **playersToTrack**: A list of player names
  - **name**: The name of the player to track
  - **raid**: A name for the main raid group of the player
  - **class**: The class of the player
  - **spec**: The class specialization of the player

## Running the gear tracker

Execute `dotnet run` in `src/fusion.geartracker`

## Running the Google Sheets reporter

Execute `dotnet run` in `src/fusion.geartracker.sheet`

## Understanding application data

- **reportsByCode**: All WCL reports known to your application.
- **playersByName**: Player information includes the report in which the player first appears and all gear acquired by the player. Each item saves the date of the first report in which the item was detected.
- **reportCodesByPlayer**: Report codes already checked by players.
