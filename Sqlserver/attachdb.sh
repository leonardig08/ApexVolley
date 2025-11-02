#!/bin/bash

# ===============================
# Script: attachdb.sh
# Scopo: Avvia SQL Server, attende che sia pronto e allega il database ApexVolleyDb
# ===============================

# Trova il percorso corretto di sqlcmd
if [ -d "/opt/mssql-tools18" ]; then
    SQLCMD_PATH="/opt/mssql-tools18/bin/sqlcmd"
elif [ -d "/opt/mssql-tools" ]; then
    SQLCMD_PATH="/opt/mssql-tools/bin/sqlcmd"
else
    SQLCMD_PATH=$(find /opt -name "sqlcmd" 2>/dev/null | head -1)
fi

echo "Using sqlcmd at: $SQLCMD_PATH"

# Avvia SQL Server in background
echo "🚀 Avvio di SQL Server..."
/opt/mssql/bin/sqlservr &
sqlserver_pid=$!

# Gestione della chiusura pulita
cleanup() {
    echo "🧹 Interruzione di SQL Server..."
    kill -TERM $sqlserver_pid 2>/dev/null
    wait $sqlserver_pid 2>/dev/null
    exit 0
}
trap cleanup SIGTERM SIGINT

# Attesa dinamica che SQL Server accetti connessioni
echo "⏳ Attesa che SQL Server accetti connessioni..."

for i in {1..30}; do
    $SQLCMD_PATH -S localhost,1433 -U SA -P "${SA_PASSWORD}" -C -Q "SELECT 1" &>/dev/null
    if [ $? -eq 0 ]; then
        echo "✅ SQL Server è pronto (tentativo $i)."
        break
    fi
    echo "⏳ Tentativo $i/30: SQL Server non ancora pronto..."
    sleep 2
done

if [ $i -eq 30 ]; then
    echo "❌ SQL Server non si è avviato in tempo (timeout 60s)."
    kill -TERM $sqlserver_pid 2>/dev/null
    wait $sqlserver_pid 2>/dev/null
    exit 1
fi

# Test di connessione finale
echo "🔍 Test connessione SQL Server..."
$SQLCMD_PATH -S localhost,1433 -U SA -P "${SA_PASSWORD}" -C -Q "SELECT GETDATE();" || {
    echo "❌ Connessione fallita. Verifica la password e lo stato del server."
    kill -TERM $sqlserver_pid 2>/dev/null
    wait $sqlserver_pid 2>/dev/null
    exit 1
}

# Attach del database se non esiste già
echo "🔗 Verifica se ApexVolleyDb è già presente..."
$SQLCMD_PATH -S localhost,1433 -U SA -P "${SA_PASSWORD}" -C -Q "
USE master;

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ApexVolleyDb')
BEGIN
    PRINT '🧩 Attach del database ApexVolleyDb...';
    BEGIN TRY
        CREATE DATABASE ApexVolleyDb
        ON
            (FILENAME = '/var/opt/mssql/data/ApexVolleyDb.mdf'),
            (FILENAME = '/var/opt/mssql/data/ApexVolleyDb_log.ldf')
        FOR ATTACH;
        PRINT '✅ Database allegato con successo!';
    END TRY
    BEGIN CATCH
        PRINT '❌ Errore durante l''attach:';
        PRINT ERROR_MESSAGE();
    END CATCH
END
ELSE
BEGIN
    PRINT '⚠️  ApexVolleyDb esiste già.';
END
"

echo "✅ SQL Server pronto su porta 1433"
wait $sqlserver_pid
