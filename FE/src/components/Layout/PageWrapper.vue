<template>
  <v-container class="page">
    <v-row class="flex-column align-center mb-5">
      <v-col cols="12">
        <v-row class="align-baseline mb-1" no-gutters>
          <h2 class="text-h5">
            <slot name="title" />
          </h2>
        </v-row>
        <v-row class="align-baseline" no-gutters>
          <v-btn color="primary" icon @click="handleBack">
            <v-icon>mdi-arrow-left</v-icon>
          </v-btn>
        </v-row>
      </v-col>
    </v-row>
    <v-row v-if="$slots.default" class="flex-column align-center mb-5 page__box">
      <slot />
    </v-row>
    <slot name="row" />
  </v-container>
</template>

<script>
import { mapGetters } from "vuex";
import { GetterNames } from "../../store/enums/vuexEnums";
import { RoleEnum } from "../../code/enums/roleEnums";
import allRoutes from "../../code/enums/routesEnum";

export default {
  name: "Wrapper",
  computed: {
    ...mapGetters({
      roles: GetterNames.GetRoles,
      isRole: GetterNames.isRole,
    }),
    isAdmin() {
      return this.isRole(RoleEnum.Admin, RoleEnum.Driver);
    },
  },
  methods: {
    handleBack() {
      this.$router.push({
        name: this.isAdmin ? allRoutes.Teleop : allRoutes.Dashboard,
      });
    },
  },
};
</script>
//
<style lang="scss" scoped>
.page {
  &__box {
    & > div {
      border-radius: 12px !important;
      // padding: 12px 16px;
      box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.07);
    }
  }
}
</style>
