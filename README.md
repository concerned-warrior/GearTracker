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
  "sheetsWeeksOldToIgnore": 12,
  "reportCountToUpdate": 500,
  "newestReportDate": "2023-05-16",
  "oldestReportDate": "2023-01-01",
  "validReportDateByZoneId": {
    "1017": "2023-01-19",
    "1018": "2023-06-08"
  },
  "reportBlacklist": []
}
```

- **appDataPath**: Where application data will be saved
- **baseAddress**: Should be `"https://www.warcraftlogs.com/api/v2/client"`
- **clientId**: Your WCL API client identifier from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **clientSecret**: Your WCL API client secret from [WCL V2 clients](https://classic.warcraftlogs.com/api/clients)
- **guildId**: The guild identifier can be found on a WCL guild reports URL
- **sheetsClientId**: Your Google client identifier
- **sheetsClientSecret**: Your Google client secret
- **sheetsSpreadsheetId**: A Google Sheets spreadsheet identifier, which can be found in the URL
- **sheetsWeeksOldToIgnore**: How many weeks to report items that are no longer being used
- **reportCountToUpdate**: How many reports to check in one run, which may save WCL API Points
- **newestReportDate**: The most recent date from which to pull data
- **oldestReportDate**: The oldest date from which to pull data
- **validReportDateByZoneId**: An object of zone identifiers -> date the raid is released
- **reportBlacklist**: Report codes to ignore

## Using the gear tracker

You can execute `dotnet run` in the `test/*` directories.

The first run will take a while, and may consume over 2,000 WCL API points, due to pulling a lot of reports and initializing many items.

1. Run `fusion.geartracker.wcl.test` to pull data from WCL.
2. Run `fusion.geartracker.update.test` to update your data store.
3. Run `fusion.geartracker.sheet.test` to generate your first Google Sheet.
4. Run `fusion.geartracker.items.test` after updating the "Known Items" tab.
5. Run `fusion.geartracker.loot.test` after updating the "Loot Dump" tab.
6. Run `fusion.geartracker.players.test` after updating the "Players" tab.
7. Repeat step 2 after running any of steps 4-6.

## Understanding application data

A `reports` directory in your `appDataPath` holds all report information and combatant info from WCL.

- **playersToTrack**: Only items found on players in this set will be reported on the Google Sheet.
- **knownItems**: Only gear found in this set will be reported on the Google Sheet.
- **playersByName**: Player information includes the report in which the player first appears and all gear acquired by the player. Each item saves the date of the first report in which the item was detected.
