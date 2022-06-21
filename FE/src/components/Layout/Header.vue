<template>
  <div>
    <v-app-bar app class="white--text" color="primary">
      <v-toolbar-title>
        <router-link :to="{ path: '/' }" class="white--text text-decoration-none">
          {{ $t("general.appName") }}
        </router-link>
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <v-select
        v-if="getMe != null"
        :items="companies"
        class="px-3 pb-0 language"
        dark
        dense
        hide-details
        item-text="name"
        item-value="id"
        label="companies"
        outlined
        @change="handleChangeTenant"
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
import { mapGetters, mapMutations } from "vuex";
import allRoutes from "../../code/enums/routesEnum";
import { LOGOUT_USER } from "../../code/graphql/queries";
import { GetterNames, MutationNames } from "../../store/enums/vuexEnums";
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
      getMe: GetterNames.GetMe,
      roles: GetterNames.GetRoles,
      isRole: GetterNames.isRole,
    }),
    isAdmin() {
      return this.isRole(RoleEnum.Admin, RoleEnum.Driver);
    },
    companies() {
      return this.getMe.tenants.nodes;
    },
  },

  methods: {
    ...mapMutations({
      setMe: MutationNames.SetMe,
    }),

    async logout() {
      try {
        await this.$apollo.query({
          query: LOGOUT_USER,
        });
        this.setMe(null);
        this.$router.push({ name: allRoutes.Login });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.user.logoutFailed"),
          type: "error",
          text: e,
        });
      }
    },

    handleChangeLang(val) {
      this.$i18n.lang = val;
      localStorage.setItem("language", val);
      this.$router.go();
    },
    handleChangeTenant(val) {
      this.multiTe = val;
      localStorage.setItem("company", val);
      console.log(val);
    },
  },
};

export class tenantsId {}
</script>

<style lang="scss" scoped>
.language {
  max-width: 200px;
}
</style>
