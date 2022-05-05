export function getStation(station) {
  if (station) {
    return {
      station: { id: station.id },
    };
  }
  return undefined;
}
