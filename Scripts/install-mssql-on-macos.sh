#!/bin/bash

SERVICES=(auth course gamification subscription)
SA_PASSWORD="QsHdvFYoDu788zw"
BASE_PORT=1434

if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Install it from https://www.docker.com/products/docker-desktop"
    exit 1
fi

for i in "${!SERVICES[@]}"; do
    CONTAINER_NAME="mssql_${SERVICES[$i]}"
    PORT=$((BASE_PORT + i))

    if docker ps -a --format '{{.Names}}' | grep -q "^$CONTAINER_NAME$"; then
        echo "🔄 MSSQL container $CONTAINER_NAME already exists. Restarting..."
        docker start $CONTAINER_NAME
    else
        echo "🚀 Starting MSSQL container $CONTAINER_NAME on port $PORT..."
        docker run --platform linux/amd64 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$SA_PASSWORD" \
          -p $PORT:1433 --name $CONTAINER_NAME -d mcr.microsoft.com/mssql/server:latest
    fi

    echo "⏳ Waiting for MSSQL $CONTAINER_NAME to be ready..."
    until docker run --rm --platform linux/amd64 mcr.microsoft.com/mssql-tools:latest /bin/bash -c \
        "/opt/mssql-tools/bin/sqlcmd -S host.docker.internal,$PORT -U sa -P '$SA_PASSWORD' -Q 'SELECT 1'" &> /dev/null; do
        echo "⏳ Waiting for MSSQL to be ready..."
        sleep 5
    done
    echo "✅ MSSQL is ready!"

    echo "📂 Checking if ${SERVICES[$i]}DB exists..."
    DB_EXISTS=$(docker run --rm --platform linux/amd64 mcr.microsoft.com/mssql-tools:latest /bin/bash -c "
        /opt/mssql-tools/bin/sqlcmd -S host.docker.internal,$PORT -U sa -P '$SA_PASSWORD' -Q 'SELECT name FROM sys.databases WHERE name = \"${SERVICES[$i]}DB\"'
    ")

    if [[ "$DB_EXISTS" == *"${SERVICES[$i]}DB"* ]]; then
        echo "✅ Database ${SERVICES[$i]}DB already exists. Skipping creation."
    else
        echo "📂 Creating database ${SERVICES[$i]}DB..."
        docker run --rm --platform linux/amd64 mcr.microsoft.com/mssql-tools:latest /bin/bash -c "
            /opt/mssql-tools/bin/sqlcmd -S host.docker.internal,$PORT -U sa -P '$SA_PASSWORD' -Q 'CREATE DATABASE [${SERVICES[$i]}DB]'
        "
        echo "✅ Database ${SERVICES[$i]}DB created successfully!"
    fi
done

echo "🎉 All MSSQL instances are running, and databases are set up!"