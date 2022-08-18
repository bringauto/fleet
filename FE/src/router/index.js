import Vue from "vue";
import VueRouter from "vue-router";
import Dashboard from "../views/Dashboard.vue";
import Teleop from "../views/Teleop.vue";
import Login from "../views/Login.vue";
import NewOrder from "../views/NewOrder.vue";
import Settings from "../views/Settings.vue";
import NewMultipleOrder from "../views/NewMultipleOrder.vue";

import { RoleEnum } from "../code/enums/roleEnums";
import allRoutes from "../code/enums/routesEnum";
import store from "../store";
import { ActionNames, GetterNames } from "../store/enums/vuexEnums";

Vue.use(VueRouter);

const routes = [
  {
    path: "/dashboard",
    name: allRoutes.Dashboard,
    component: Dashboard,
    meta: {
      roles: [RoleEnum.User, RoleEnum.Privileged, RoleEnum.Admin],
      defaultRoles: [RoleEnum.User, RoleEnum.Privileged],
    },
  },
  {
    path: "/teleop",
    name: allRoutes.Teleop,
    component: Teleop,
    meta: {
      roles: [RoleEnum.Driver, RoleEnum.Admin],
      defaultRoles: [RoleEnum.Driver, RoleEnum.Admin],
    },
  },
  {
    path: "/login",
    name: allRoutes.Login,
    component: Login,
  },
  {
    path: "/new-order",
    name: allRoutes.NewOrder,
    component: NewOrder,
    meta: {
      roles: [RoleEnum.User, RoleEnum.Privileged, RoleEnum.Admin, RoleEnum.Driver],
      defaultRoles: [],
    },
  },
  {
    path: "/new-multiple-order",
    name: allRoutes.NewMultipleOrder,
    component: NewMultipleOrder,
    meta: {
      roles: [RoleEnum.User, RoleEnum.Privileged, RoleEnum.Admin, RoleEnum.Driver],
      defaultRoles: [],
    },
  },
  {
    path: "/edit-order/:id",
    name: allRoutes.EditOrder,
    component: NewOrder,
    meta: {
      roles: [RoleEnum.User, RoleEnum.Privileged, RoleEnum.Admin, RoleEnum.Driver],
      defaultRoles: [],
    },
  },
  {
    path: "/settings",
    name: allRoutes.Settings,
    component: Settings,
    meta: {
      roles: [RoleEnum.Privileged, RoleEnum.Admin, RoleEnum.Driver],
      defaultRoles: [],
    },
  },
];

const router = new VueRouter({
  mode: "history",
  routes,
});

async function getMe() {
  if (store.getters[GetterNames.GetMe] === null) {
    await store.dispatch(ActionNames.GetMe);
  }
}

router.beforeEach(async (to, from, next) => {
  // Check if user has me if not it tries to get me
  await getMe();
  // Go to login if not logged in if not going from login page
  if (store.getters[GetterNames.GetMe] == null) {
    const goTo = to.name !== allRoutes.Login ? { name: allRoutes.Login } : undefined;
    return next(goTo);
  }

  const userRoles = store.getters[GetterNames.GetRoles];
  const hasPermission = userRoles.some((role) => to.meta?.roles?.includes(role));
  // Find default route
  const defaultRoute = routes.find((route) =>
    userRoles.some((role) => route.meta?.defaultRoles?.includes(role))
  );
  const redirect = !hasPermission ? { name: defaultRoute?.name ?? allRoutes.Dashboard } : undefined;
  return next(redirect);
});

export default router;
