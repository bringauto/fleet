<template>
  <v-container class="login">
    <v-row class="mb-10 justify-center align-center text-center">
      <v-col cols="10" sm="4">
        <h3 class="title mt-3 mb-4">{{ $t("login.title") }}</h3>
        <ValidationObserver v-slot="{ handleSubmit }" ref="form">
          <form novalidate @submit.prevent="handleSubmit(onSubmit)">
            <v-row>
              <v-col cols="12">
                <ValidationProvider
                  v-slot="{ errors }"
                  vid="username"
                  rules="required"
                  name="$t('login.username')"
                >
                  <v-text-field
                    id="username"
                    v-model.lazy="userName"
                    full-width
                    :label="$t('login.username')"
                    outline
                    hide-details
                    type="username"
                    name="username"
                    class="ma-0 pa-0"
                    data-at="username"
                  />

                  <div v-if="errors[0]" class="login__input--error mt-0 pt-0">
                    {{ $t("login.required") }}
                  </div>
                </ValidationProvider>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="12">
                <ValidationProvider
                  v-slot="{ errors }"
                  vid="password"
                  rules="required"
                  name="$t('login.password')"
                >
                  <v-text-field
                    id="password"
                    v-model.lazy="password"
                    full-width
                    :label="$t('login.password')"
                    hide-details
                    outline
                    name="password"
                    type="password"
                    class="ma-0 pa-0"
                    data-at="password"
                  />
                  <div v-if="errors[0]" class="login__input--error mt-0 pt-0">
                    {{ $t("login.required") }}
                  </div>
                </ValidationProvider>
                <div v-if="error" class="login__credentials subheading mt-3 pt-0">
                  {{ error }}
                </div>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="12">
                <v-btn
                  color="primary"
                  type="submit"
                  large
                  data-at="login-button"
                  class="primary mt-3"
                >
                  <div>{{ $t("login.submit") }}</div>
                </v-btn>
              </v-col>
            </v-row>
          </form>
        </ValidationObserver>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapMutations } from "vuex";
import { ValidationProvider, ValidationObserver } from "vee-validate/dist/vee-validate.full";
import { LOGIN_USER } from "../code/graphql/queries";
import { MutationNames } from "../store/enums/vuexEnums";

export default {
  name: "Login",
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data: () => ({
    userName: "",
    password: "",
    error: null,
  }),
  methods: {
    ...mapMutations({
      setMe: MutationNames.SetMe,
    }),
    async onSubmit() {
      try {
        const { data } = await this.$apollo.query({
          query: LOGIN_USER,
          variables: {
            password: this.password,
            userName: this.userName,
          },
        });
        if (data?.UserQuery?.login) {
          this.setMe(data && data.UserQuery.login);
          this.$router.push({ path: "/" });
        } else {
          this.$notify({
            group: "global",
            title: this.$i18n.tc("notifications.user.loginFailed"),
            type: "error",
          });
        }
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.user.loginFailed"),
          type: "error",
          text: e,
        });
      }
    },
  },
};
</script>

<style lang="scss">
@import "@/assets/styles/variables.scss";

.login {
  background: $white !important;

  &__input {
    &--error {
      color: $danger;
    }
  }

  &__credentials {
    text-align: center;
    font-weight: 600;
    color: $danger;
  }
}
</style>
