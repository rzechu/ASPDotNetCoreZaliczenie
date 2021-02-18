# ASPDotNetCoreZaliczenie

Aplikacja na zaliczenie z przedmiotu Programowanie w œrodowisku ASP.NET

Wykorzystane technologie i frameworki
- C# ASP.NET Core 3.1
- Entity Framework 6
- SQLite

Zewnêtrzne
- API OpenWeatherMap

Aplikacja ma za zadanie pos³u¿yæ jako domowy "dashboard".
W tym momencie sk³ada siê z 2 modu³ów.

Dostêp do obydwóch modu³ów wymaga autoryzacji/posiadania odpowiednich Cookies lub zalogowania siê, dziêki czemu zostan¹ one utworzone i zweryfikowane.

1. Weather 
Sprawdzanie pogody w kilku wybranych miejscowoœciach. (W oparaciu o API OpenWeather Map).
Wymagany jest klucz APIKey w pliku appsettings.json 
"OpenWeatherAPI": "getYourOwnAPIKey"

W przypadku nie wprowadzenia swojego klucza/kluczu domyœlnym pojawia siê stosowny komunikat.


2. PriceTracker
Modu³ s³u¿¹cy do œledzenia cen.
Wskazujemy stronê, która nas interesuje oraz jej element XPath, w którym umieszczona jest cena.
Mo¿na dodaæ wiele rekordów, które s¹ zapisywane w bazie SQLite

Nastêpnie klikamy na dole Check Values, po chwili powinny pojawiæ siê aktualne wartoœci:
Przyk³ad


