#!/bin/bash
set -e

echo ">>> Iniciando API de tareas médicas..."
dotnet run --project TasksApi --urls "http://0.0.0.0:5000"
