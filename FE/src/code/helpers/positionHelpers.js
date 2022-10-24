export function getLatLong(position) {
  let latLong = position.replaceAll(/[^0-9-.,]/g, "");
  latLong = latLong.split(",");
  if (latLong[0] && latLong[1]) {
    return { latitude: Number(latLong[0]), longitude: Number(latLong[1]) };
  }
  if (!latLong[1] && latLong[0]) {
    return { latitude: Number(latLong[0]), longitude: 0 };
  }
  return { latitude: 0, longitude: 0 };
}

export function getLat(position) {
  const getLatid = position.latitude;
  if (getLatid[0]) {
    return { latitude: Number(getLatid[0]) };
  }
  return { latitude: 0 };
}

export function getLong(position) {
  const getLongit = position.longitude;
  if (getLongit[0]) {
    return { longitude: Number(getLongit[0]) };
  }
  return { longitude: 0 };
}

export function getPositionValue(station) {
  const { latitude, longitude } = station;
  if (latitude && longitude) {
    return `${latitude}, ${longitude}`;
  }
  if (latitude && !longitude) {
    return `${latitude}, 0`;
  }
  return `${0}, ${0}`;
}

export function justNumber(event, keys = []) {
  const enabledKeys = ["Backspace", "Control", "c", "v", ...keys];
  if (event.key.match(/^[a-zA-Z]*$/) && !enabledKeys.includes(event.key)) {
    event.preventDefault();
  }
}
