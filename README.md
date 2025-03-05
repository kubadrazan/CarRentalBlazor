# MiNICarRental
This is college project made for course Developing web apps with .Net at Warsaw University of Technology. This project consists of car rental API and web application that communicates with this API and other to manage car rentals. Api and web app use separate sql databases. There's user and employee. User can rent from all APIs and see all his rentals made through our app, not other. Employee manages all rental made with our API. 
Tech-stack:
* Blazor for front-end.
* Redis for caching
* Azure for hosting app (and sql database, blob storage and secrets)
* Mssql server for testing
* OAuth for authentication
* SendGrid as smtp server
* Azure DevOps with GIT for managing project
* EntityFramework
