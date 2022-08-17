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
  },
  mutations: {
    [MutationNames.SetMe](state, user) {
      state.user = user;
    },
    [MutationNames.SetTenant](state, tenant) {
      state.selectedTenant = tenant;
    },
  },
  actions: {
    [ActionNames.GetMe]: async ({ commit }) => {
      try {
        const { data } = await apolloClient.query({ query: GET_ME });
        console.log(data);
        commit(MutationNames.SetMe, data && data.UserQuery.me);
        commit(MutationNames.SetTenant, data && data.UserQuery.me.tenants.nodes[0]);
      } catch (error) {
        console.error(error);
      }
    },
    [ActionNames.SetUser]: ({ commit }, user) => {
      commit(MutationNames.SetMe, user);
      commit(MutationNames.SetTenant, user ? user.tenants.node[0] : null);
    },
  },
});
