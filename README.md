# ApexVolley 🏐

ApexVolley è un sito web per la gestione e la presentazione di una squadra di pallavolo. Il progetto è sviluppato in C# utilizzando ASP.NET Core MVC.

Caratteristiche

- Calendario delle partite e allenamenti
- Profilo dei giocatori con statistiche
- Sezione news per aggiornamenti della squadra
- Galleria foto
- Classifiche e risultati aggiornati
- Area riservata per staff e giocatori

Tecnologie usate

- ASP.NET Core MVC
- C#
- Razor Pages / Views
- Entity Framework Core (per la gestione del database)
- SQL Server / SQLite
- Bootstrap 5 (per lo stile)
- Identity (per l'autenticazione)

Come iniziare

Prerequisiti

- .NET SDK 7.0+
- Un editor come Visual Studio 2022 o Visual Studio Code
- (Opzionale) SQL Server o SQLite

Clona il progetto

git clone https://github.com/tuo-username/ApexVolley.git
cd ApexVolley

Avvia il progetto

dotnet run


Migrazioni del database

dotnet ef database update

Struttura del progetto

ApexVolley/
├── Controllers/      # Controller MVC
├── Models/           # Modelli dati
├── Views/            # Razor Views
├── wwwroot/          # File statici (CSS, JS, immagini)
├── Data/             # Configurazione del database e Identity
├── Program.cs        # Punto d'ingresso dell'app
├── Startup.cs        # Configurazione servizi e middleware
└── appsettings.json  # Configurazioni generali

Prossime funzionalità

- Sezione eventi con prenotazioni
- Statistiche avanzate per ogni partita
- Integrazione con un'app mobile
- Dashboard per amministratori

Licenza

Questo progetto è distribuito sotto licenza MIT. Vedi il file LICENSE per maggiori dettagli.

Made with ❤️ by la squadra ApexVolley
