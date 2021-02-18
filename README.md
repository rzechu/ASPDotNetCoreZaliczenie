# ASPDotNetCoreZaliczenie

Aplikacja na zaliczenie z przedmiotu Programowanie w �rodowisku ASP.NET

Wykorzystane technologie i frameworki
- C# ASP.NET Core 3.1
- Entity Framework 6
- SQLite

Zewn�trzne
- API OpenWeatherMap

Aplikacja ma za zadanie pos�u�y� jako domowy "dashboard".
W tym momencie sk�ada si� z 2 modu��w.

Dost�p do obydw�ch modu��w wymaga autoryzacji/posiadania odpowiednich Cookies lub zalogowania si�, dzi�ki czemu zostan� one utworzone i zweryfikowane.

1. Weather 
Sprawdzanie pogody w kilku wybranych miejscowo�ciach. (W oparaciu o API OpenWeather Map).
Wymagany jest klucz APIKey w pliku appsettings.json 
"OpenWeatherAPI": "getYourOwnAPIKey"

W przypadku nie wprowadzenia swojego klucza/kluczu domy�lnym pojawia si� stosowny komunikat.


2. PriceTracker
Modu� s�u��cy do �ledzenia cen.
Wskazujemy stron�, kt�ra nas interesuje oraz jej element XPath, w kt�rym umieszczona jest cena.
Mo�na doda� wiele rekord�w, kt�re s� zapisywane w bazie SQLite

Nast�pnie klikamy na dole Check Values, po chwili powinny pojawi� si� aktualne warto�ci:
Przyk�ad


