# SubredditListener

SubredditListener is a tool developed to continuously monitor a subrreddit in near real time. 

## Running application

clone the repository 

```bash
git clone https://github.com/dinet/SubRedditListener.git
```
Navigate to SubRedditListner folder

```bash
cd SubRedditListener\SubRedditListner
```
Update appsettings.json file with your ClientId, ClientSecret and AgentName
```json
"ApiConfig": {
  "ClientId": "",
  "ClientSecret": "",
  "AgentName": "",
  "TokenUrl": "https://www.reddit.com/api/v1/",
  "BaseUrl": "https://oauth.reddit.com/",
  "SubRedditName": "diy",
  "StatRetrivalInterval": 2000
}
```
Run following commands
```bash
dotnet run
