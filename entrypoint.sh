#!/bin/bash

# Resolve "matchmaker" into IP
host_ip=$(getent hosts matchmaker | awk '{print $1}')

# Check if the host_ip is not empty
if [ -n "$host_ip" ]; then
  # Update config JSON file with IP
  jq --arg host_ip "$host_ip" '.MatchMakerConnectionOptions.Ip = $host_ip' appsettings.json > temp.json && mv temp.json appsettings.json
  echo "JSON file updated successfully."
else
  echo "Error: Unable to resolve the hostname."
  exit 1
fi

dotnet /app/castledice-game-server.dll
