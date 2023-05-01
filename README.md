# Fusion Gear Tracker

Use [Warcraft Logs](https://www.warcraftlogs.com) (WCL) to discover when gear is acquired by players in a guild.

## Prerequisites

- [Download and Install .NET 7](https://dotnet.microsoft.com/download)

### Application settings

Make a `src/fusion.geartracker/appsettings/appsettings.json` file with the following fields:

```json
{
  "appDataPath": "appdata/data.json",
  "baseAddress": "https://www.warcraftlogs.com/api/v2/client",
  "clientId": "{your client id}",
  "clientSecret": "{your client secret}",
  "guildId": 123456,
  "firstReportDate": "2023-05-01",
  "lastReportDate": "2023-01-01",
  "itemsToTrack": [
    45516,
    45536,
    45931
  ],
  "playersToTrack": [
    "Feralbestclass",
    "Blizzhateswars"
  ]
}
```

- **appDataPath**: Where application data will be saved
- **baseAddress**: Should be `"https://www.warcraftlogs.com/api/v2/client"`
- **clientId**: Your WCL API client identifier from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **clientSecret**: Your WCL API client secret from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **guildId**: The guild identifier can be found on a WCL guild reports URL
- **firstReportDate**: The most recent date from which to pull data
- **lastReportDate**: The oldest date from which to pull data
- **itemsToTrack**: A list of item identifiers found on [Wowhead](https://www.wowhead.com)
- **playersToTrack**: A list of player names

## Running the gear tracker

Execute `dotnet run` in `src/fusion.geartracker`

## Understanding application data

- **reportsByCode**: All WCL reports known to your application.
- **playersByName**: Player information includes the report in which the player first appears and all gear acquired by the player. Each item saves the date of the first report in which the item was detected.
- **reportCodesByPlayer**: Report codes already checked by players.
