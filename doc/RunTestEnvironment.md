
# BringAuto Fleet Test deploy

Prerequisites

- GNU/Linux system
- docker >= 21.10
- docker-compose >= 1.29
- python3 >= 3.9
- pip3

## Run BringAuto Etna

- `git clone https://github.com/bringauto/etna.git`
- `cd etna`
- `docker-compose --profile=without-ba-fleet up`

## Build BringAuto Fleet App

- `git clone https://github.com/bringauto/fleet.git`
- `cd industrial-portal`
- `docker-compose build`
- `docker-compose up`

## Initialize Test Database

- `git clone https://github.com/bringauto/fleet-init.git`
- `cd fleet-init`
- `pip3 install -r requirements.txt`
- `python3 main.py`

## Connect to BringAuto Fleet
- Log into industrial portal on http://127.0.0.1:8011 using default account and password (Admin, Admin1)
