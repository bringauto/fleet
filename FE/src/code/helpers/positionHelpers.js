export function getLatLong(position) {
  const latLong = position.split(",");

  return {
    latitude: Number(latLong[0] || 0),
    longitude: Number(latLong[1] || 0),
  };
}

export function getPositionValue(station) {
  const { latitude, longitude } = station;

  return `${Number(latitude || 0)}, ${Number(longitude || 0)}`;
}

export function justNumber(event, keys = []) {
  const enabledKeys = ["Backspace", ...keys];
  if (event.key.match(/^[a-zA-Z]*$/) && !enabledKeys.includes(event.key)) {
    event.preventDefault();
  }
}
