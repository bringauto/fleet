 # Station

Station is a place where the car can load/unload cargo.

Properties

- `name: string`. Name of the Station that will be shown in the GUI
- `contactPhone: string`. Contact phone of the station used for Twilio notification
- `latitude: float`. Latitude of the station ([WGS84])
- `longitude: float`. longitude of the station ([WGS84])
- `id: int`. Station database ID

Warning: not all attributes can be changed by GraphQL API.

[WGS84]: https://en.wikipedia.org/wiki/World_Geodetic_System
