import Vue from "vue";
import * as Sentry from "@sentry/browser";
import { Vue as VueIntegration } from "@sentry/integrations";
import Notifications from "vue-notification";

import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import i18n from "./plugins/i18n/i18n";
import { apolloProvider } from "./plugins/apollo";
import "leaflet/dist/leaflet.css";
import "./code/helpers/validations";
import "roboto-fontface/css/roboto/roboto-fontface.css";
import "@mdi/font/css/materialdesignicons.css";

Sentry.init({
  dsn: process.env.APP_VUE_SENTRY_URL,
  integrations: [new VueIntegration({ Vue, attachProps: true })],
});

Vue.config.productionTip = false;
Vue.use(Notifications);

new Vue({
  i18n,
  router,
  store,
  apolloProvider,
  vuetify,
  render: (h) => h(App),
}).$mount("#app");
