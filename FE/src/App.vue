<template>
  <v-app class="app">
    <app-header />
    <v-main>
      <notifications group="global" position="top right" />
      <div>
        <router-view />
      </div>
    </v-main>
  </v-app>
</template>

<script>
import { localize } from "vee-validate";
import cs from "vee-validate/dist/locale/cs.json";
import AppHeader from "./components/Layout/Header.vue";

localize("cs", cs);
export default {
  name: "App",

  components: {
    AppHeader,
  },
};
</script>

<style lang="scss">
.box-wrapper {
  border-radius: 12px !important;
  background: #f6f6fb;
  padding: 12px 16px;
  box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.07);
}
.vue-notification {
  padding: 10px;
  margin: 5px;

  font-size: 12px;

  color: #ffffff;
  background: #44a4fc;
  border-left: 5px solid #187fe7;
  border-radius: 5px !important;
  box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.07);

  &.warn {
    background: #ffb648;
  }

  &.error {
    background: #ff5252;
  }

  &.success {
    background: #4caf50;
  }
}
</style>
