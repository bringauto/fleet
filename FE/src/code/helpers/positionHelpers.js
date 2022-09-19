export function getLatLong(position) {
  let latLong = position.replaceAll(/[^0-9-.,]/g, "");
  latLong = latLong.split(",");
  console.log(latLong);
  if (latLong[0] && latLong[1]) {
    return { latitude: Number(latLong[0]), longitude: Number(latLong[1]) };
  }
  if (!latLong[1] && latLong[0]) {
    return { latitude: Number(latLong[0]), longitude: 0 };
  }
  return { latitude: 0, longitude: 0 };
}

export function getPositionValue(station) {
  const { latitude, longitude } = station;
  if (latitude && longitude) {
    console.log(latitude);
    return `${latitude}, ${longitude}`;
  }
  if (latitude && !longitude) {
    return `${latitude}, 0`;
  }
  return `${0}, ${0}`;
}

export function justNumber(event, keys = []) {
  const enabledKeys = ["Backspace", ...keys];
  if (event.key.match(/^[a-zA-Z]*$/) && !enabledKeys.includes(event.key)) {
    event.preventDefault();
  }
}
