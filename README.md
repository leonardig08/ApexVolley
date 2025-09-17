# ApexVolley 🏐

Sito web per la squadra di pallavolo **ApexVolley**, sviluppato in **C#** con **ASP.NET Core MVC**, completamente dockerizzato.

---

## Caratteristiche
 
- Profilo dei giocatori con statistiche  
- Sezione news per aggiornamenti della squadra  
- Risultati aggiornati
- Palmares trofei
- Area riservata per staff e giocatori  

---

## Tecnologie usate

- ASP.NET Core MVC  
- C#  
- Razor Pages / Views  
- SQL Server / SQLite  
- Bootstrap 5 (per lo stile)  
- Identity (autenticazione)  
- Docker
- Docker-compose
- Stripe
- Stripe CLI

---

## Come Avviare il sito web

## ATTENZIONE LO SCRIPT BASH PER ATTACCARE IL DB DEVE ESSERE PRIMA CONFIGURATO CON TERMINAZIONI DI RIGA UNIX

### E NECESSARIO INSTALLARE STRIPE CLI

1. Andare nella root del progetto
```batch
docker-compose up
```


2. Aprire un altro cmd e con Stripe CLI installato eseguire
```batch
stripe listen --forward-to http://localhost:8080/api/stripe/webhook
```

3. Per far funzionare correttamente il pagamento fittizio mettere in appsettings.json il proprio codice webhook che è univoco per l'account

```json
"Stripe": {
  "SecretKey": "sk_test_...",
  "PublishableKey": "pk_test_...",
  "WebhookSecret": "whsec_..."
}
```




Aprire nel browser sulla porta 8080
