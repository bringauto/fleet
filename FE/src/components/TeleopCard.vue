<template>
  <div class="teleop-card">
    <template v-if="car">
      <v-select
        :value="car"
        :label="$t('newOrder.car')"
        :items="cars"
        item-text="name"
        item-value="id"
        required
        @change="$emit('setCar', $event)"
      />
      <div class="d-flex justify-center align-center text-caption mb-1">
        <span v-if="car.fuel" class="mr-2">
          <v-icon>{{ getCarBatteryIcon(car.fuel.toFixed(4)) }}</v-icon>
          {{ car.fuel.toFixed(4) * 100 }}%
        </span>
        <span>{{ getLastUpdate(car) }}</span>
      </div>
      <v-btn
        block
        class="text--center mb-2"
        color="primary"
        small
        text
        @click="showOrder = !showOrder"
      >
        {{ $t("orders.title") }}
      </v-btn>
      <v-row align="center" class="mb-1" justify="center">
        <p class="text-h4 mb-0 mr-3 teleop-card__lenght" @click="showOrder = !showOrder">
          {{ car.orders.nodes.length }}
        </p>
        <!-- button of single Order
        <v-btn icon color="primary" @click="handleNewOrder">
          <v-icon> mdi-plus-circle-outline</v-icon>
        </v-btn> -->
        <v-btn :to="{ name: allRoutes.NewMultipleOrder }" color="primary" icon>
          <v-icon> mdi-plus-circle-multiple-outline</v-icon>
        </v-btn>
      </v-row>
      <template v-for="(order, key) in car.orders.nodes.slice(0, 3)">
        <p :key="order.id" class="text-caption mb-0">{{ key + 1 }}. {{ orderListing(order) }}</p>
      </template>
      <p v-if="car.orders.nodes.length > 3 || cars[0]" class="text-caption mb-0">...</p>
      <!-- select box for car status
      <v-select
        class="mt-2"
        :value="car.status"
        :items="CarStateFormated"
        item-text="trans"
        item-value="status"
        :label="$t('general.status')"
        outlined
        hide-details
        dense
        @input="$emit('set-car-status', { status: $event, car: car })"
      />
      -->
      <v-dialog v-model="showOrder" width="800">
        <v-card>
          <v-card-title class="headline primary white--text">
            {{ $t("general.orders") }}
          </v-card-title>

          <v-card-text class="pt-10">
            <v-row class="align-baseline box-wrapper mb-4" no-gutters>
              <template v-if="sortOrders && sortOrders.length > 0">
                <template v-for="order in sortOrders">
                  <v-col :key="order.id" cols="12">
                    <v-row class="mb-5 align-baseline" no-gutters>
                      <v-col cols="12" sm="3">{{ orderListing(order) }}</v-col>
                      <v-col cols="12" sm="4">
                        <p v-if="order.user" class="mb-1">
                          {{ $t("general.user") }}: {{ order.user.userName }}
                        </p>
                        <p class="mb-3">
                          {{ $t("general.priority") }}:
                          <span :class="`${getPriorityEnum(order.priority).color}--text`">
                            {{ getPriorityEnum(order.priority).trans }}
                          </span>
                        </p>
                      </v-col>
                      <v-col align="left" cols="12" sm="4">
                        <v-select
                          :append-icon="isAdmin ? '$dropdown' : ''"
                          :items="OrderStateFormated"
                          :label="$t('general.status')"
                          :value="order.status"
                          disabled
                          class="mb-2"
                          dense
                          hide-details
                          item-text="trans"
                          item-value="status"
                          outlined
                          @input="$emit('set-order-status', { status: $event, order, car })"
                        />
                      </v-col>
                      <v-col align="center" cols="12" sm="3">
                        <v-btn
                          color="error"
                          @click="$emit('set-order-status', { status: 'Canceled', order, car })"
                        >
                          {{ $t("orders.cancel") }}
                        </v-btn>
                      </v-col>
                      <v-col align="center" cols="12" sm="1">
                        <v-btn
                          :icon="!$vuetify.breakpoint.mobile"
                          color="error"
                          small
                          @click="removeOrder(order.id)"
                        >
                          <v-icon small> mdi-delete</v-icon>
                        </v-btn>
                      </v-col>
                    </v-row>
                    <v-divider v-if="$vuetify.breakpoint.mobile" class="mb-10" />
                  </v-col>
                </template>
              </template>
              <template v-else>
                <v-row class="mb-5 align-baseline" no-gutters>
                  <v-col class="text-center" cols="12">
                    <h2 class="text-subtitle-2">{{ $t("orders.none") }}</h2>
                  </v-col>
                </v-row>
              </template>
            </v-row>
          </v-card-text>

          <v-divider></v-divider>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" text @click="showOrder = false">
              {{ $t("general.close") }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </template>
    <v-skeleton-loader v-else type="article" />
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { CarStateFormated, getCarState } from "../code/enums/carEnums";
import { getPriorityEnum } from "../code/enums/prioEnum";
import { OrderStateFormated } from "../code/enums/orderEnums";
import { orderApi } from "../code/api";
import { orderListing } from "../code/helpers/orderHelpers";
import { getTime, getLastUpdate } from "../code/helpers/timeHelpers";
import { getCarBatteryIcon } from "../code/helpers/carHelpers";
import allRoutes from "../code/enums/routesEnum";
import { RoleEnum } from "../code/enums/roleEnums";
import { GetterNames } from "../store/enums/vuexEnums";

export default {
  name: "CarCard",
  props: {
    car: {
      type: Object,
      default: null,
    },
    cars: {
      type: Array,
      default: () => [],
    },
  },
  data: () => ({
    CarStateFormated,
    OrderStateFormated,
    collapsed: true,
    showOrder: false,
    allRoutes,
  }),
  computed: {
    ...mapGetters({
      getTenant: GetterNames.GetTenant,
      getMe: GetterNames.GetMe,
      roles: GetterNames.GetRoles,
      isRole: GetterNames.isRole,
    }),

    isAdmin() {
      return this.isRole(RoleEnum.Admin);
    },
    isDriver() {
      return this.isRole(RoleEnum.Driver);
    },
    isUser() {
      return this.isRole(RoleEnum.User);
    },
    companies() {
      return this.$data;
    },
    filteredCars() {
      return this.cars.filter((car) => (car.underTest && this.isAdmin) || !car.underTest);
    },
    sortOrders() {
      const filtered = this.car.orders.nodes;
      return filtered.sort((a, b) => {
        const aVal = this.getPriorityEnum(a.priority).value;
        const bVal = this.getPriorityEnum(b.priority).value;
        if (aVal > bVal) {
          return -1;
        }
        return aVal === bVal ? 0 : 1;
      });
    },
  },
  methods: {
    getPriorityEnum,
    getCarState,
    orderListing,
    getLastUpdate,
    getCarBatteryIcon,

    formatTime(val) {
      return getTime(val, "d.M.y k:m");
    },
    async removeOrder(id) {
      try {
        await orderApi.deleteOrder(id);
        this.$emit("get-cars");
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.delete"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.deleteFailed"),
          type: "error",
        });
        console.error(e);
      }
    },
    handleNewOrder() {
      this.$router.push({
        name: allRoutes.NewOrder,
        params: {
          carId: this.car.id,
        },
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.teleop-card {
  border-radius: 12px !important;
  background: #f6f6fb;
  padding: 12px 16px;
  box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.07);
  min-width: 300px;
  max-width: 300px;
  @media (max-width: 768px) {
    max-width: 300px;
    margin: -80px auto 0;
  }
  @media (max-width: 481px) {
    min-width: 80%;
    width: 80%;
    margin: -80px auto 30px;
  }

  &__lenght {
    cursor: pointer;
  }

  .v-skeleton-loader__article.v-skeleton-loader__bone {
    background: #f6f6fb !important;
  }
}
</style>
