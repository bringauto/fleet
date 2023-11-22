export function getLatLong(position) {
  const latLong = position.split(",");

  return {
    latitude: Number(latLong[0] || 0),
    longitude: Number(latLong[1] || 0),
  };
}

export function getPositionValue(station) {
  const { latitude, longitude } = station;

  if (latitude && longitude) {
    return `${latitude}, ${longitude}`;
  }

  return `${latitude || 0}, ${longitude || 0}`;
}

export function justNumber(event, keys = []) {
  const enabledKeys = ["Backspace", ...keys];
  if (event.key.match(/^[a-zA-Z]*$/) && !enabledKeys.includes(event.key)) {
    event.preventDefault();
  }
}
