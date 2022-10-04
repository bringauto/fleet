<template>
  <v-col cols="12" md="8">
    <ValidationObserver v-slot="{ handleSubmit }" ref="form">
      <form novalidate @submit.prevent="handleSubmit(editig ? updateOrder : onSubmit)">
        <ValidationProvider
          v-slot="{ errors }"
          vid="carId"
          rules="required"
          :name="$t('newOrder.car')"
        >
          <v-select
            v-model="carId"
            :label="$t('newOrder.car')"
            :items="cars"
            item-text="name"
            item-value="id"
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>

        <ValidationProvider
          v-slot="{ errors }"
          vid="stationTo"
          rules="required"
          :name="$t('newOrder.stationTo')"
        >
          <v-select
            v-model="stationTo"
            :label="$t('newOrder.stationTo')"
            :items="stations"
            item-text="name"
            item-value="id"
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>
        <ValidationProvider v-slot="{ errors }" vid="selectedPrio" :name="$t('newOrder.priority')">
          <v-select
            v-model="selectedPrio"
            :label="$t('newOrder.priority')"
            :items="priorities"
            item-text="trans"
            item-value="priority"
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>
        <div class="mt-5">
          <v-btn large color="success" class="mr-4" type="submit">
            {{ $t("login.submit") }}
          </v-btn>
        </div>
      </form>
    </ValidationObserver>
  </v-col>
</template>

<script>
import { mapGetters } from "vuex";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { carApi, stationApi, orderApi } from "../../code/api";
import { getPrioEnumAccordingToRole } from "../../code/enums/prioEnum";
import allRoutes from "../../code/enums/routesEnum";
import { getTime, formatArrive } from "../../code/helpers/timeHelpers";
import { GetterNames } from "../../store/enums/vuexEnums";

export default {
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      stations: [],
      cars: [],
      priorities: [],
      stationFrom: undefined,
      arrive: null,
      stationTo: null,
      selectedPrio: null,
      carId: null,
      editedOrder: null,
      menu: false,
    };
  },
  computed: {
    ...mapGetters({
      roles: GetterNames.GetRoles,
      isAdmin: GetterNames.isAdmin,
    }),
    editig() {
      return !!this.$route.params.id;
    },
  },
  watch: {
    cars: {
      handler(val) {
        if (val.length === 1 && !this.carId) {
          this.carId = val[0].id;
        }
      },
      deep: true,
      immediate: true,
    },
    priorities: {
      handler(val) {
        if (val.length > 0 && !this.selectedPrio) {
          this.selectedPrio = val[val.length - 1].priority;
        }
      },
      deep: true,
      immediate: true,
    },
  },
  async mounted() {
    await this.initForm();
  },

  methods: {
    async initForm() {
      this.stations = await stationApi.getStations();
      const cars = await carApi.getCarsWithoutHistory();

      this.cars = cars.filter((car) => (car.underTest && this.isAdmin) || !car.underTest);
      this.priorities = getPrioEnumAccordingToRole(this.$store.state.user.roles);
      if (this.editig) {
        const order = await orderApi.getOrder({ id: this.$route.params.id });
        if (order) {
          this.carId = order.car.id;
          this.stationFrom = order.from?.id;
          this.stationTo = order.to.id;
          this.selectedPrio = order.priority;
          this.editedOrder = order;
          this.arrive = order.arrive ? getTime(order.arrive) : null;
        } else {
          this.$router.push("/");
        }
      } else {
        this.stationTo = this.$route.params.stationTo;
        this.carId = this.$route.params.carId;
      }
    },
    async onSubmit() {
      const dto = {
        carId: this.carId,
        fromStationId: this.stationFrom,
        priority: this.selectedPrio,
        toStationId: this.stationTo,
      };
      dto.arrive = formatArrive(this.arrive);
      try {
        await orderApi.addOrder(dto);
        this.$router.push({ name: this.isAdmin ? allRoutes.Teleop : allRoutes.Dashboard });
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.create"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.createFailed"),
          type: "error",
        });
        console.error(e);
      }
    },
    async updateOrder() {
      const dto = {
        ...this.editedOrder,
        car: this.carId,
        from: this.stationFrom,
        priority: this.selectedPrio,
        to: this.stationTo,
      };
      dto.arrive = this.formatArrive();
      try {
        await orderApi.updateOrder(dto);
        this.$router.push({ name: this.isAdmin ? allRoutes.Teleop : allRoutes.Dashboard });
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.update"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.updateFailed"),
          type: "error",
        });
        console.error(e);
      }
    },
  },
};
</script>
