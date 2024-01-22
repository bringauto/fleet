
# BringAuto Fleet Test deploy

Prerequisites

- docker >= 20.10 (for Windows link [here](https://docs.docker.com/desktop/install/windows-install/))
- docker-compose >= 1.29
- python3 >= 3.9
- pip3
- git (for Windows link [here](https://git-scm.com/download/win) - during installation, configure line ending conversions **checkout as-is, commit as-is**)

## Run BringAuto Etna (1.2.1)

- `git clone https://github.com/bringauto/etna.git`
- `cd etna`
- `docker-compose --profile=without-fleet up`

## Build BringAuto Fleet App

- `git clone https://github.com/bringauto/fleet.git`
- `cd fleet`
- `docker-compose build`
- `docker-compose up`

## Initialize Test Database

- `git clone https://github.com/bringauto/fleet-init.git`
- `cd fleet-init`
- `pip3 install -r requirements.txt`
- `python main.py`

## Connect to BringAuto Fleet
- Log into Fleet on http://127.0.0.1:8011 using default account and password (Admin, Admin1)
