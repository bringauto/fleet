
# Industrial Portal Test deploy

Prerequisites

- GNU/Linux system
- docker >= 21.10
- docker-compose >= 1.29
- python3 >= 3.9
- pip3

## Run BringAuto Etna

- `git clone https://github.com/bringauto/etna.git`
- `cd etna`
- `docker-compose --profile=without-industrial-portal up`

## Build Industrial Portal App

- `git clone https://github.com/bringauto/industrial-portal.git`
- `cd industrial-portal`
- `docker-compose build`
- `docker-compose up`

## Initialize Test Database

- `git clone https://github.com/bringauto/industrial-portal-init.git`
- `cd industrial-portal-init`
- `pip3 install -r requirements.txt`
- `python3 main.py`