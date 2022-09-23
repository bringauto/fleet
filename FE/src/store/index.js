import Vue from "vue";
import Vuex from "vuex";
import { apolloClient } from "../plugins/apollo";
import { ActionNames, GetterNames, MutationNames } from "./enums/vuexEnums";
import { GET_ME } from "../code/graphql/queries";
import { RoleEnum } from "../code/enums/roleEnums";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    user: null,
    selectedTenant: null,
    stations: null,
    selectedCarId: null,
  },
  getters: {
    [GetterNames.GetAuthStatus](state) {
      return !!state.user;
    },
    [GetterNames.GetMe](state) {
      return state.user;
    },
    [GetterNames.GetRoles](state) {
      return state.user?.roles ?? [];
    },
    [GetterNames.isAdmin](state) {
      return state.user?.roles.includes(RoleEnum.Admin) ?? false;
    },
    [GetterNames.isDriver](state) {
      return state.user?.roles.includes(RoleEnum.Driver) ?? false;
    },
    [GetterNames.isRole]: (state) => (roles) => {
      return state.user?.roles.some((role) => roles.includes(role)) ?? false;
    },
    [GetterNames.GetTenant](state) {
      return state.selectedTenant;
    },
    [GetterNames.GetSelectCar](state) {
      return state.selectedCarId;
    },
    [GetterNames.isFirstStation](state) {
      if (state.user.stops[0].nodes) {
        return state.user.stops[0].nodes;
      }
      return { longitude: 47.09713, latitude: 37.54337 };
    },
  },
  mutations: {
    [MutationNames.SetMe](state, user) {
      state.user = user;
    },
    [MutationNames.SetTenant](state, tenant) {
      state.selectedTenant = tenant;
      sessionStorage.setItem("selectedTenant", JSON.stringify(tenant));
    },
    [MutationNames.SetCarId](state, id) {
      state.selectedCarId = id;
    },
    [MutationNames.LoadTenant](state, tenant) {
      const storedTenant = sessionStorage.getItem("selectedTenant");
      if (storedTenant) {
        state.selectedTenant = JSON.parse(storedTenant);
      } else {
        state.selectedTenant = tenant;
      }
    },
  },
  actions: {
    [ActionNames.GetMe]: async ({ commit }) => {
      try {
        const { data } = await apolloClient.query({ query: GET_ME });
        commit(MutationNames.SetMe, data && data.UserQuery.me);
        commit(MutationNames.LoadTenant, data && data.UserQuery.me.tenants.nodes[0]);
      } catch (error) {
        console.error(error);
      }
    },
    [ActionNames.SetUser]: ({ commit }, user) => {
      commit(MutationNames.SetMe, user);
      commit(MutationNames.LoadTenant, user ? user.tenants.node[0] : null);
    },
  },
});
