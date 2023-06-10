
how to view the log of the web app from a linux terminal:
sudo journalctl -u boardbrawl.service -f

Notes on uploading to the app server
To upload files with pscp, it requires Pageant to be running on local PC and adding the private auth key to it.
I'd reccomend using VS Code to upload files instead, but I have not configured it yet.

Before uploading, connect to the app server and stop the web app with:
sudo systemctl stop boardbrawl.service

After uploading, restart the web app with:
sudo systemctl start boardbrawl.service

And then check the status of the web app with:
sudo systemctl status boardbrawl.service

You can view the logging for the web app with:
sudo journalctl -u boardbrawl.service -f
Press Ctrl+C to stop showing the log

Upload commands
These commands can be ran from a developer command prompt

Make sure to publish first, and check the path to the publish folder with your environment:
Command to upload everything in the publish folder to the server: 
pscp -r "F:\source\repos\BoardBrawl\src\BoardBrawl.WebApp.MVC\bin\Publish\*" "mcoffey@134.209.230.164:/var/www/boardbrawl"

Command to only upload app dlls (much smaller upload)
pscp -r "F:\source\repos\BoardBrawl\src\BoardBrawl.WebApp.MVC\bin\Publish\BoardBrawl*.dll" "mcoffey@134.209.230.164:/var/www/boardbrawl"
	