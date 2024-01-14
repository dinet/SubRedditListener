# SubredditListener

SubredditListener is a tool developed to continuously monitor a subrreddit in near real time. 

## Running application

Use Git to clone the repository 

```bash
https://github.com/dinet/SubRedditListener.git
```
Navigate to SubRedditListner folder

```bash
cd [yourfolder]\SubRedditListener\SubRedditListner
```
Update appsettings.json file your ClientId, ClientSecret and AgentName
```json
"ApiConfig": {
  "ClientId": "",
  "ClientSecret": "",
  "AgentName": "",
  "TokenUrl": "https://www.reddit.com/api/v1/",
  "BaseUrl": "https://oauth.reddit.com/",
  "SubRedditName": "diy"
}
```
Run following commands
```bash
dotnet run
