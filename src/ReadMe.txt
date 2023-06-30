
how to view the log of the web app from a linux terminal:
sudo journalctl -u boardbrawl.service -f

Notes on uploading to the app server
I'd reccomend using VS Code to upload files

Before uploading, connect to the app server and stop the web app with:
sudo systemctl stop boardbrawl.service

After uploading, restart the web app with:
sudo systemctl start boardbrawl.service

And then check the status of the web app with:
sudo systemctl status boardbrawl.service

You can view the logging for the web app with:
sudo journalctl -u boardbrawl.service -f
Press Ctrl+C to stop showing the log

Creating migrations:
from PM console in VS, with the BoarBrawl.Data project selected as the default project
	and with the BoardBrawl.WebApp.MVC project selected as the Startup project
Delete migrations folder first and then run a command like this:
Add-Migration InitialCreate -Context IdentityDbContext -OutputDir Identity\Migrations
Add-Migration InitialCreate -Context ApplicationDbContext -OutputDir Application\Migrations

Not sure what happens on the Digital Ocean droplet, but occasionally, .Net gets corrupted
See this for correction - https://stackoverflow.com/questions/73753672/a-fatal-error-occurred-the-folder-usr-share-dotnet-host-fxr-does-not-exist

sudo apt remove dotnet*
sudo apt remove aspnetcore*
sudo apt remove netstandard*
sudo apt update
sudo apt install dotnet-sdk-6.0