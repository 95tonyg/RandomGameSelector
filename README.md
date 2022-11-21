# RandomGameSelector
ASP.Net MVC project where a user can add games to a list, then can press a button to randomly select a game for them to play!
To use, either create a LocalDB server called ProjectModels, and create a db called RandomGameSelectorDB before running "Update-Database" in the Package Manager Console.
You can also just switch the connection string in appsettings.json and create a db with a different name. Migrations will create a Game, Genre, and GameGenre table.
It will also seed some data into those tables. Once that is finished you should be able to run the project.
