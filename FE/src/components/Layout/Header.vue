<template>
  <div>
    <v-app-bar app color="primary" class="white--text">
      <v-toolbar-title>
        <router-link :to="{ path: '/' }" class="white--text text-decoration-none">
          {{ $t("general.appName") }}
        </router-link>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-select
        v-model="$i18n.locale"
        :items="languages"
        dark
        dense
        hide-details
        outlined
        class="px-5 pb-0 language"
        @change="handleChangeLang"
      />
      <v-btn v-if="isAdmin" icon small color="white" class="mr-3" :to="{ name: settings }">
        <v-icon small color="white">mdi-cog</v-icon>
      </v-btn>
      <v-btn v-if="getMe" icon small color="white" class="mr-3" @click="logout">
        <v-icon small color="white">mdi-logout</v-icon>
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
  },
};
</script>

<style scoped lang="scss">
.language {
  max-width: 200px;
}
</style>
