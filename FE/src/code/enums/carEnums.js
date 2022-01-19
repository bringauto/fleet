import i18n from "../../plugins/i18n/i18n";

export const CarState = Object.freeze({
  WAITING: "WAITING",
  ACCEPTIONORDER: "ACCEPTIONORDER",
  DRIVING: "DRIVING",
  WAITFORLOAD: "WAITFORLOAD",
  OUTOFSERVICE: "OUTOFSERVICE",
  CHARGING: "CHARGING",
});

export const CarStateFormated = [
  {
    status: CarState.WAITING,
    trans: i18n.tc("satuses.waiting"),
  },
  {
    status: CarState.ACCEPTIONORDER,
    trans: i18n.tc("satuses.acceptionOrder"),
  },
  {
    status: CarState.DRIVING,
    trans: i18n.tc("satuses.driving"),
  },
  {
    status: CarState.WAITFORLOAD,
    trans: i18n.tc("satuses.waitingForLoad"),
  },
  {
    status: CarState.OUTOFSERVICE,
    trans: i18n.tc("satuses.outOfService"),
  },
  {
    status: CarState.CHARGING,
    trans: i18n.tc("satuses.charging"),
  },
];

export const getCarState = (status) => {
  return CarStateFormated.find((s) => s.status === status);
};
