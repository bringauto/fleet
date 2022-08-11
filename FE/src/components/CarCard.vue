<template>
  <v-row class="flex-column align-center">
    <v-col cols="12" class="box-wrapper">
      <v-row no-gutters class="justify-space-between align-baseline mb-4">
        <h2 class="text-h5">{{ car.name }}</h2>
        <v-chip outlined color="primary">
          {{ carState(car.status) }}
        </v-chip>
      </v-row>
      <v-row no-gutters class="justify-space-between align-baseline">
        <v-col cols="12">
          <v-row class="flex-column align-center">
            <v-btn large color="success" class="mb-2" @click="$router.push('new-order')">{{
              $t("general.order")
            }}</v-btn>
            <v-dialog v-model="showOrder" width="800">
              <template #activator="{}">
                <v-btn large text color="primary" @click="showOrder = !showOrder">{{
                  $t("general.showOrders")
                }}</v-btn>
              </template>

              <v-card>
                <v-card-title class="headline primary white--text">
                  {{ $t("general.orders") }}
                </v-card-title>

                <v-card-text class="mt-2">
                  <div v-if="getFilteredOrders(car.id) === undefined">
                    {{ $t("general.noOrders") }}
                  </div>
                  <v-data-table
                    v-else
                    hide-default-footer
                    :headers="headers"
                    :items="getFilteredOrders(car.id)"
                    :items-per-page="-1"
                    class="elevation-0 mt-2"
                  >
                    <template #[`item.actions`]="{ item }">
                      <v-btn small color="primary" icon fab @click="handleEditOrder(item)">
                        <v-icon small> mdi-pencil </v-icon>
                      </v-btn>
                      <v-btn small color="error" icon fab @click="handleDeleteOrder(item)">
                        <v-icon small> mdi-delete </v-icon>
                      </v-btn>
                    </template>
                    <template #[`item.arrive`]="{ item }">
                      <span v-if="item.arrive">{{ getTime(item.arrive) }}</span>
                    </template>
                  </v-data-table>
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
          </v-row>
        </v-col>
      </v-row>
    </v-col>
  </v-row>
</template>

<script>
import { getCarState, CarStateFormated } from "../code/enums/carEnums";
import { getOrderState } from "../code/enums/orderEnums";
import { orderApi } from "../code/api";
import allRoutes from "../code/enums/routesEnum";
import { getTime } from "../code/helpers/timeHelpers";

export default {
  name: "CarCard",
  props: {
    car: {
      type: Object,
      required: true,
    },
  },
  data: () => ({
    CarStateFormated,
    orders: [],
    showOrder: false,
    headers: [
      {
        text: "Auto",
        align: "start",
        sortable: false,
        value: "car.name",
      },
      { text: "Ze stanice", value: "from.name" },
      { text: "Do stanice", value: "to.name" },
      { text: "Čas", value: "arrive" },
      { text: "Status", value: "trans" },
      { text: "Akce", value: "actions", sortable: false },
    ],
  }),
  computed: {
    carState() {
      return (state) => {
        const { trans } = this.getCarState(state);
        return trans;
      };
    },
    getFilteredOrders() {
      return (id) => {
        const filteredOrders = this.orders.filter((o) => o.car.id === id);
        return filteredOrders.map((order) => {
          const { trans } = getOrderState(order.status);
          return { ...order, trans };
        });
      };
    },
  },
  async mounted() {
    await this.getAllOrders();
  },
  methods: {
    getTime,
    getCarState,
    handleEditOrder(item) {
      this.$router.push({ name: allRoutes.EditOrder, params: { id: item.id } });
    },
    async getAllOrders() {
      try {
        this.orders = await orderApi.getOrders();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
          text: e,
        });
      }
    },
    async handleDeleteOrder(order) {
      try {
        await orderApi.deleteOrder(order.id);
        this.showOrder = false;
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
          text: e,
        });
      }
      await this.getAllOrders();
    },
  },
};
</script>

<style></style>
