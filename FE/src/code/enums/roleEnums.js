import i18n from "../../plugins/i18n/i18n";

export const RoleEnum = Object.freeze({
  Admin: "Admin",
  Driver: "Driver",
  Privileged: "Privileged",
  User: "User",
});

export const RoleEnumFormated = [
  {
    role: RoleEnum.Admin,
    trans: i18n.tc("roles.admin"),
  },
  {
    role: RoleEnum.Driver,
    trans: i18n.tc("roles.driver"),
  },
  {
    role: RoleEnum.Privileged,
    trans: i18n.tc("roles.privileged"),
  },
  {
    role: RoleEnum.User,
    trans: i18n.tc("roles.user"),
  },
];

export const getRoleEnum = (role) => {
  return RoleEnum.find((s) => s.role === role);
};
