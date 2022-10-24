<template>
  <div>
    <v-app-bar app class="white--text" color="primary">
      <v-toolbar-title>
        <router-link :to="{ path: '/' }" class="white--text text-decoration-none">
          {{ $t("general.appName") }}
        </router-link>
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <v-text-field
        v-if="getTenant != null"
        solo
        outlined
        disabled
        dense
        hide-details
        class="px-3 pb-0 single-line language white--text text-decoration-none"
        readonly
        :value="computedMe.userName"
      />
      <v-select
        v-if="getTenant != null"
        v-model="selectedTenant"
        :append-icon="isAdmin ? '$dropdown' : ''"
        :disabled="!isAdmin"
        :items="companies"
        class="px-3 pb-0 language"
        dark
        dense
        hide-details
        item-text="name"
        item-value="id"
        :label="$t('general.companyName')"
        outlined
        return-object
      />
      <v-select
        v-model="$i18n.locale"
        :items="languages"
        class="px-5 pb-0 language"
        dark
        dense
        hide-details
        outlined
        @change="handleChangeLang"
      />
      <v-btn v-if="isAdmin" :to="{ name: settings }" class="mr-3" color="white" icon small>
        <v-icon color="white" small>mdi-cog</v-icon>
      </v-btn>
      <v-btn v-if="getMe" class="mr-3" color="white" icon small @click="logout">
        <v-icon color="white" small>mdi-logout</v-icon>
      </v-btn>
      <!-- <v-app-bar-nav-icon color="white" @click.stop="drawer = !drawer"></v-app-bar-nav-icon> -->
    </v-app-bar>
    <!-- <v-navigation-drawer v-model="drawer" app right>
      <v-list dense>
        <v-list-item link>
          <v-list-item-content>
            <v-list-item-title>Home</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link>
          <v-list-item-content>
            <v-list-item-title>Contact</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer> -->
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import allRoutes from "../../code/enums/routesEnum";
import { LOGOUT_USER } from "../../code/graphql/queries";
import { ActionNames, GetterNames, MutationNames } from "../../store/enums/vuexEnums";
import { RoleEnum } from "../../code/enums/roleEnums";

export default {
  name: "Header",
  data: () => ({
    drawer: false,
    settings: allRoutes.Settings,
    languages: ["cs", "en"],
  }),
  computed: {
    ...mapGetters({
      computedMe: GetterNames.GetMe,
      roles: GetterNames.GetRoles,
      isRole: GetterNames.isRole,
      getTenant: GetterNames.GetTenant,
    }),
    isAdmin() {
      return this.isRole(RoleEnum.Admin, RoleEnum.Driver);
    },
    isDriver() {
      return this.isRole(RoleEnum.User);
    },
    companies() {
      return this.computedMe?.tenants?.nodes ?? [];
    },
    selectedTenant: {
      get() {
        return this.getTenant;
      },
      set(val) {
        this.setTenant(val);
        this.$router.go();
      },
    },
  },

  methods: {
    ...mapActions({
      setUser: ActionNames.SetUser,
      getMe: ActionNames.GetMe,
    }),
    ...mapMutations({
      setTenant: MutationNames.SetTenant,
    }),
    async logout() {
      try {
        await this.$apollo.query({
          query: LOGOUT_USER,
        });

        this.setTenant(null);
        this.setUser(null);
        this.getMe(null);
        sessionStorage.clear();

        this.$router.push({ name: allRoutes.Login });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.user.logoutFailed"),
          type: "error",
        });
        console.error(e);
      }
    },

    handleChangeLang(val) {
      this.$i18n.lang = val;
      localStorage.setItem("language", val);
      this.$router.go();
    },
  },
};
</script>

<style lang="scss" scoped>
.language {
  max-width: 200px;
}
</style>
