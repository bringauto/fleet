export function getStation(station) {
  if (station) {
    return {
      station: { id: station.id },
      latitude: station.latitude,
      longitude: station.longitude,
    };
  }
  return undefined;
}
