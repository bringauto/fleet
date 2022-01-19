export const DOWNLOAD_INTERVAL = 2500;
export const DEFAULT_STATION = Object.freeze({ latitude: 0, longitude: 0 });
export const DEFAULT_ROUTE = Object.freeze({
  name: "",
  color: "#000000",
  stops: [{ longitude: 0, latitude: 0, order: 0 }],
});
export const DEFAULT_CAR = Object.freeze({
  carAdminPhone: "",
  companyName: "",
  hwId: "",
  name: "",
  routeId: undefined,
  underTest: false,
});
