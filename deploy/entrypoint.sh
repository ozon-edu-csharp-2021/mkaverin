#!/bin/bash

set -e
run_cmd="dotnet OzonEdu.MerchandiseService.dll --no-build -v d"

dotnet OzonEdu.MerchandiseService.Migrator.dll --no-build -v d -- --dryrun

dotnet OzonEdu.MerchandiseService.Migrator.dll --no-build -v d

>&2 echo "Merchandise DB Migrations complete, starting app."
>&2 echo "Run Merchandise: $run_cmd"
exec $run_cmd