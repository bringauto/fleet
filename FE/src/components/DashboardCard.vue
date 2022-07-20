<template>
  <div class="dash-card">
    <template v-if="car">
      <p class="text-center text-h6 mb-0">{{ car.name }}</p>
      <div class="d-flex justify-center align-center text-caption mb-1">
        <span v-if="car.fuel" class="mr-2">
          <v-icon>{{ getCarBatteryIcon(car.fuel) }}</v-icon> {{ car.fuel }}%
        </span>
        <span>{{ getLastUpdate(car) }}</span>
      </div>
      <v-btn block class="text--center" color="primary" small text @click="showOrder = !showOrder">
        {{ $t("general.orders") }}
      </v-btn>
      <v-row align="center" class="px-3" justify="center">
        <p class="text-h4 mb-0 mr-3 dash-card__length" @click="showOrder = !showOrder">
          {{ car.orders.nodes.length }}
        </p>
        <!--
        <v-btn :disabled="car.underTest" color="primary" icon @click="handleNewOrder">
          <v-icon> mdi-plus-circle-outline</v-icon>
        </v-btn>
        <v-btn
          :disabled="car.underTest"
          :to="{ name: allRoutes.NewMultipleOrder }"
          color="primary"
          icon
        >
          <v-icon> mdi-plus-circle-multiple-outline</v-icon>
        </v-btn>
        -->
      </v-row>
      <template v-for="(order, key) in car.orders.nodes.slice(0, 3)">
        <p :key="order.id" class="text-caption mb-0">{{ key + 1 }}. {{ orderListing(order) }}</p>
      </template>

      <p v-if="car.orders.nodes.length > 3" class="text-caption mb-0">...</p>
      <v-dialog v-model="showOrder" width="1000">
        <v-card>
          <v-card-title class="headline primary white--text">
            {{ $t("general.orders") }}
          </v-card-title>
          <v-card-text class="mt-2">
            <div v-if="car.orders.nodes === undefined">
              {{ $t("general.noOrders") }}
            </div>
            <v-data-table
              v-else
              :headers="headers"
              :items="getFilteredOrders"
              :items-per-page="-1"
              class="box-wrapper my-2"
              hide-default-footer
            >
              <template v-slot:[`item.actions`]="{ item }">
                <v-btn class="mr-2" color="primary" icon small @click="handleEditOrder(item)">
                  <v-icon small> mdi-pencil</v-icon>
                </v-btn>
                <v-btn color="error" icon small @click="handleDeleteOrder(item)">
                  <v-icon small> mdi-delete</v-icon>
                </v-btn>
              </template>
              <template v-slot:[`item.arrive`]="{ item }">
                <span v-if="item.arrive">{{ getTime(item.arrive) }}</span>
                <span v-else>{{ $t("newOrder.soon") }}</span>
              </template>
              <template v-slot:[`item.priority`]="{ item }">
                <span :class="`${getPriorityEnum(item.priority).color}--text`">
                  {{ getPriorityEnum(item.priority).trans }}
                </span>
              </template>
            </v-data-table>
            <v-row justify="center">
              <v-btn :disabled="car.underTest" color="success" text @click="handleNewOrder">
                {{ $t("orders.new") }}
              </v-btn>
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
import { orderApi } from "../code/api";
import { CarStateFormated, getCarState } from "../code/enums/carEnums";
import { getOrderState } from "../code/enums/orderEnums";
import { orderListing } from "../code/helpers/orderHelpers";
import allRoutes from "../code/enums/routesEnum";
import { getLastUpdate, getTime } from "../code/helpers/timeHelpers";
import { getPriorityEnum } from "../code/enums/prioEnum";
import { getCarBatteryIcon } from "../code/helpers/carHelpers";

export default {
  name: "DashCard",
  props: {
    // eslint-disable-next-line vue/require-default-prop
    car: {
      type: Object,
    },
  },
  data() {
    return {
      CarStateFormated,
      orders: [],
      showOrder: false,
      allRoutes,
      headers: [
        {
          text: this.$i18n.tc("tables.car"),
          align: "start",
          sortable: false,
          value: "name",
        },
        { text: this.$i18n.tc("tables.toStation"), value: "to.name" },
        { text: this.$i18n.tc("tables.arrive"), value: "arrive" },
        { text: this.$i18n.tc("tables.status"), value: "trans" },
        { text: this.$i18n.tc("tables.priority"), value: "priority" },
        { text: this.$i18n.tc("tables.actions"), value: "actions", sortable: false },
      ],
    };
  },
  computed: {
    carState() {
      return (state) => {
        const { trans } = this.getCarState(state);
        return trans;
      };
    },
    getFilteredOrders() {
      return this.car.orders.nodes.map((order) => {
        const { trans } = getOrderState(order.status);
        return { ...order, trans, name: this.car.name };
      });
    },
  },
  methods: {
    getTime,
    getCarState,
    orderListing,
    getLastUpdate,
    getPriorityEnum,
    getCarBatteryIcon,
    handleEditOrder(item) {
      this.$router.push({ name: allRoutes.EditOrder, params: { id: item.id } });
    },
    handleNewOrder() {
      this.$router.push({
        name: allRoutes.NewOrder,
        params: {
          carId: this.car.id,
        },
      });
    },

    async handleDeleteOrder(order) {
      try {
        await orderApi.deleteOrder(order.id);
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
  },
};
</script>

<style lang="scss">
.dash-card {
  border-radius: 12px !important;
  background: #f6f6fb;
  padding: 12px 16px;
  box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.07);
  min-width: 300px;
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
