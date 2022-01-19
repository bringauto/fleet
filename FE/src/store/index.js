import Vue from "vue";
import Vuex from "vuex";
import { apolloClient } from "../plugins/apollo";
import { ActionNames, MutationNames, GetterNames } from "./enums/vuexEnums";
import { GET_ME } from "../code/graphql/queries";
import { RoleEnum } from "../code/enums/roleEnums";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    user: null,
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
    [GetterNames.isRole]: (state) => (roles) => {
      return state.user?.roles.some((role) => roles.includes(role)) ?? false;
    },
  },
  mutations: {
    [MutationNames.SetMe](state, user) {
      state.user = user;
    },
  },
  actions: {
    [ActionNames.GetMe]: async ({ commit }) => {
      try {
        const { data } = await apolloClient.query({ query: GET_ME });

        commit(MutationNames.SetMe, data && data.UserQuery.me);
      } catch (error) {
        console.error(error);
      }
    },
  },
});
