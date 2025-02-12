#!/bin/bash

echo "🔍 Searching for MSSQL containers running"
MSSQL_CONTAINERS=$(docker ps -a --filter "ancestor=mcr.microsoft.com/mssql/server" --format "{{.ID}} {{.Names}} {{.Ports}}" | grep -E "1434|1435|1436")

if [ -z "$MSSQL_CONTAINERS" ]; then
    echo "✅ No MSSQL containers found. Exiting..."
    exit 0
fi

echo "🛑 The following MSSQL containers are running"
echo "$MSSQL_CONTAINERS"
echo ""

read -p "⚠️ Do you really want to delete these MSSQL containers? (yes/no): " CONFIRM
if [[ "$CONFIRM" != "yes" ]]; then
    echo "❌ Operation canceled."
    exit 0
fi

echo "🚀 Stopping and removing the selected MSSQL containers..."
docker ps -a --filter "ancestor=mcr.microsoft.com/mssql/server" --format "{{.ID}} {{.Ports}}" | grep -E "1434|1435|1436" | awk '{print $1}' | xargs docker rm -f

echo "✅ Selected MSSQL containers have been removed!"